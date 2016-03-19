using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TSO.BA.WebApp.Models;
using Microsoft.AspNet.Identity;

namespace TSO.BA.WebApp.Controllers
{
    public class BuildingController : Controller
    {
        private FloorContext _floorContext;
        private UserManager<ApplicationUser> _userManager;

        public BuildingController(
            FloorContext argFloorContext,
            UserManager<ApplicationUser> argUserManager)
        {
            _floorContext = argFloorContext;
            _userManager = argUserManager;
        }

        public IActionResult Control(LoginViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("LogIn", "Account");

            return View(model);
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
