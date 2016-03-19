using TSO.BA.WebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.Hosting;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using TOS.BA.Shared;

namespace TSO.BA.WebApp.Controllers
{

    public class DataReceiverController : Controller
    {
        private IHubContext _machineStateHubContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IFloorContext _siteContext;

        public DataReceiverController(
            UserManager<ApplicationUser> argUserManager,
            ILogger<DataReceiverController> argLogger,
            IConnectionManager argConnectionManager,
            IFloorContext argSiteContext)
        {
            // check if the Datapoint is already in the database
            _userManager = argUserManager;
            _logger = argLogger;
            
            // SignalR3
            _machineStateHubContext = (argConnectionManager.GetHubContext<BuildingControlHub>());

            ////SignalR2
            //_machineStateHubContext = GlobalHost.ConnectionManager.GetHubContext<MachineStateHub>();

            _siteContext = argSiteContext;
        }

        [HttpPost]
        public async Task<ActionResult> PostMaterialStates([FromBody] BuildingStateListUpdateModel argBuildingStates)
        {
            if (!(await checkHeaderAuthorization()))
                return Json("Bad Authorization!");

            if (!createSiteIfNotExisting(argBuildingStates.FloorId))
                return this.HttpBadRequest("Error creating Site in db!");

            Floor floor = _siteContext.Floors.Include(f => f.BuildingStates).Single(f => f.FloorName == argBuildingStates.FloorId);

            foreach (var buildingState in argBuildingStates.Values)
            {
                if (!floor.BuildingStates.Any(ms => ms.BuildingStateKey == buildingState.BuildingStateId))
                {
                    floor.BuildingStates.Add(new BuildingState(buildingState.BuildingStateId, buildingState.BuildingStateValue));
                }
                else
                {
                    floor.BuildingStates.Single(b => b.BuildingStateKey == buildingState.BuildingStateId)
                        .BuildingStateValue = buildingState.BuildingStateValue;
                }
            }
            try
            {
                _siteContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception save changes to machine state db context: " + ex.Message);
                return this.HttpBadRequest("Error saving MachineStateDBContext!");
            }

            #region ToDo: Activate as Clients.Group works on IIS
            //var siteId = Helpers.GetSiteNameFromUserString(userAndPassword[0]);
            // as Context.User.Identity.Name is non case sensitive Clients also need to do so
            //siteId = siteId.ToLower();
            //_machineStateHubContext.Clients.All.updateMachineStates(argMachineStates);
            //var clients = _machineStateHubContext.Clients.Group(siteId);
            //_logger.LogInformation("Send machine states to group named " + siteId);
            //_machineStateHubContext.Clients.Group(siteId).updateMachineStates(argMachineStates.Values);
            //_machineStateHubContext.Clients.All.updateMachineStates(argMachineStates.Values);
            //_machineStateHubContext.Clients.Group(siteId).showMessageOnClient("Send Machine States", "Error");
            //_logger.LogWarning("Successfully posted " + argMachineStates.Values.Count() + " MachineStates to siteId " + siteId);
            #endregion

            _logger.LogInformation(argBuildingStates.Values.Count() + " MachineStates posted to site " + argBuildingStates.FloorId);

            return Json("Success");
        }
        
        #region Helpers

        private async Task<bool> checkHeaderAuthorization()
        {
            string[] userAndPassword = getUserNameAndPasswordFromHeader();
            ActionResult check = await checkUserNameAndPassword(userAndPassword[0], userAndPassword[1]);
            if (check != null)
            {
                _logger.LogError("Error Posting Messages: User " + userAndPassword[0] + " with Password " + userAndPassword[1] + " not Authorized!");
                return false;
            }
            return true;

        }

        private string[] getUserNameAndPasswordFromHeader()
        {
            string authorization = this.Request.Headers["Authorization"];
            string userAndPassword = ((authorization.Split((new char[] { ' ' }))[1]));
            userAndPassword = Encoding.UTF8.GetString(Convert.FromBase64String(userAndPassword));
            string[] nameAndPwdArray = userAndPassword.Split(new char[] { ':' });
            return nameAndPwdArray;
        }

        private async Task<ActionResult> checkUserNameAndPassword(string argUserName, string argPassword)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByNameAsync(argUserName);
                bool userOK = await _userManager.CheckPasswordAsync(user, argPassword);

                if (!userOK)
                {
                    return this.HttpUnauthorized();
                }
                return null;
            }
            catch (Exception ex0)
            {
                throw ex0;
            }
        }

        private bool createSiteIfNotExisting(string argFloorId)
        {
            if (!_siteContext.Floors.Any(f => f.FloorName == argFloorId))
            {
                _siteContext.Floors.Add(new Floor(argFloorId));
                try
                {
                    _siteContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Exception save changes to floor state db context: " + ex.Message);
                    return false;
                }
            }
            return true;
        }
        
        #endregion


    }
    
}
