using ControllerRebinder.Common.Moddels.Configurations.SubModelsOfConfigurations;
using ControllerRebinder.Common.Moddels.DTOs;
using ControllerRebinder.Core.Caches;
using ControllerRebinder.ElectronView.Models;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ControllerRebinder.ElectronView.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var config = ConfigCache.Configurations;
            return View(config.LeftJoyStick);
        }

        [HttpPost]
        public IActionResult Settings(LeftStickDTO leftJoyStick)
        {
            var config = ConfigCache.Configurations.LeftJoyStick;

            config.StaticArea = leftJoyStick.StaticArea;
            config.ThreshHoldAreaCal = leftJoyStick.ThreshHoldAreaCal;
            config.MaxValController = leftJoyStick.MaxValController;
            config.ForwardDown = leftJoyStick.ForwardDown;
            config.LeftRight = leftJoyStick.LeftRight;
            config.DeadZone = leftJoyStick.DeadZone;

            config.Controlls.Up = leftJoyStick.Up;
            config.Controlls.Down = leftJoyStick.Down;
            config.Controlls.Left = leftJoyStick.Left;
            config.Controlls.Right = leftJoyStick.Right;

            var data = JsonConvert.SerializeObject(config);
            System.IO.File.WriteAllText("appsettings.json", data);

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
