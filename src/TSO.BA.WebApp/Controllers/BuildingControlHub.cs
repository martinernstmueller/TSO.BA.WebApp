//using KNZ.KIS.Shared;
using TSO.BA.WebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;
//using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOS.BA.Shared;

namespace TSO.BA.WebApp.Controllers
{
    public interface IBuildingControlClient
    {
        void updateMachineStates(object argBuildingStates);
        void showMessageOnClient(string argMessage, string argType);
    }

    [HubName("BuildingControlHub")]
    public class BuildingControlHub : Hub<IBuildingControlClient>
    {
        public ILogger _logger { get; set; }
        IFloorContext _floorContext { get; set; }
        
        public BuildingControlHub(ILogger<DataReceiverController> argLogger, IFloorContext argFloorContext)
        {
            _logger = argLogger;
            _logger.LogInformation("Instance of BuildingControlHub created...");
            _floorContext = argFloorContext;
        }

        
        public void updateMachineStates(object argMachineStates)
        {
            //var siteId = Helpers.GetSiteNameFromUserString(Context.User.Identity.Name);
            
            //this.Clients.Group(Helpers.MachineStategroupNamePrefix + "@" + siteId).updateMachineStates(argMachineStates);
        }

        // Get all Messages from the DB using my username and the corresponding site id
        public IEnumerable<BuildingStateUpdateModel> getAllBuildingStatesFromMySite()
        {
            //var siteId = Helpers.GetSiteNameFromUserString(Context.User.Identity.Name);
            var statesWithoutFloorId = new List<BuildingStateUpdateModel>();

            if (!_floorContext.Floors.Any(f => f.FloorName == "Floor1"))
                return statesWithoutFloorId;

            Floor floor = _floorContext.Floors.Include(f => f.BuildingStates).Single(f => f.FloorName == "Floor1");

            var states = _floorContext.Floors.Include(f => f.BuildingStates).Where(f => f.FloorName == "Floor1").Single().BuildingStates;
            foreach (var state in states)
                statesWithoutFloorId.Add(new BuildingStateUpdateModel(state.BuildingStateKey, state.BuildingStateValue));
            
            return statesWithoutFloorId;
        }

        // Overridable hub methods
        public override Task OnConnected()
        {
            if ((Context.User != null) && !string.IsNullOrEmpty(Context.User.Identity.Name))
            {
                _logger.LogInformation("User " + Context.User.Identity.Name + "will  Connect");
                //var siteId = Helpers.GetSiteNameFromUserString(Context.User.Identity.Name);
                //_logger.LogInformation("Add User with ConnectionId " + Context.ConnectionId + " to Group " + siteId);
                // add the user to the corrensponding group
                //Groups.Add(Context.ConnectionId, siteId);
                //Clients.Group(siteId).showMessageOnClient("Client with id " + Context.ConnectionId + " added to group " + siteId, "Error");
            }
            
            return base.OnConnected();
        }
        public override Task OnReconnected()
        {
            //_userCount++;
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            //_userCount--;
            if ((Context.User != null) && !string.IsNullOrEmpty(Context.User.Identity.Name))
            {
                //var siteId = Helpers.GetSiteNameFromUserString(Context.User.Identity.Name);
                //_logger.LogInformation("Remove User with ConnectionId " + Context.ConnectionId + " from Group " + siteId);
                //Groups.Remove(Context.ConnectionId, siteId);
            }
            return base.OnConnected();
        }
    }
    
}
