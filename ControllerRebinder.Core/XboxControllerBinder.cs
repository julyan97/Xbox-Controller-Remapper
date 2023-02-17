using ControllerRebinder.Common.Enumerations;
using ControllerRebinder.Core.Caches;
using ControllerRebinder.Core.Services;
using ControllerRebinder.Core.Services.Imp;
using DXNET.XInput;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using WindowsInput;

namespace ControllerRebinder.Core
{
    public class XboxControllerBinder
    {
        private readonly Controller _controller;
        private readonly InputSimulator _inputSimulator;
        private readonly ILogger<XboxControllerBinder> _logger;
        private readonly IJoyStickService _LeftjoyStickService;
        private readonly IJoyStickService _RightjoyStickService;
        private readonly IButtonsService _ButtonsService;

        public XboxControllerBinder(Controller controller, InputSimulator inputSimulator, ILogger<XboxControllerBinder> logger)
        {
            InitCaches();

            _controller = controller;
            _inputSimulator = inputSimulator;
            _logger = logger;

            //Instantiate logic for the left joystick from configurations
            _LeftjoyStickService = new JoyStickService(
                _controller,
                _inputSimulator,
                JoyStick.Left,
                logger);

            //Instantiate logic for the right joystick from configurations
            _RightjoyStickService = new JoyStickService(
                _controller,
                _inputSimulator,
                JoyStick.Right,
                logger);

            //Instantiate logic for the buttons from configurations
            _ButtonsService = new ButtonsService(
                _controller, _inputSimulator,
                ConfigCache.Configurations.Buttons,
                ConfigCache.Configurations.Log);
        }

        private void InitCaches()
        {
            ConfigCache.Init();
            QuadrantCache.Init();
        }



        public async Task Start()
        {
            if(ConfigCache.Configurations.LeftJoyStick.On)
            {
                Task.Run(async () => await _LeftjoyStickService.Start());
            }
            if(ConfigCache.Configurations.RightJoyStick.On)
            {
                Task.Run(async () => await _RightjoyStickService.Start());
            }
            if(ConfigCache.Configurations.Buttons.On)
            {
                Task.Run(async () => await _ButtonsService.Start());
            }

            Console.ReadLine();
        }


    }
}

