using ControllerRebinder.Common.Enumerations;
using ControllerRebinder.Core.Caches;
using ControllerRebinder.Core.Events;
using ControllerRebinder.Core.Services;
using ControllerRebinder.Core.Services.Imp; // Add this line to include the handler namespace
using DXNET.XInput;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WindowsInput;

namespace ControllerRebinder.Core
{
    public class XboxControllerBinder
    {
        private readonly Controller _controller;
        private readonly InputSimulator _inputSimulator;
        private readonly ILogger<XboxControllerBinder> _logger;
        private readonly IJoyStickService _leftJoyStickService;
        private readonly IJoyStickService _rightJoyStickService;
        private readonly IButtonsService _buttonsService;

        public XboxControllerBinder(Controller controller, InputSimulator inputSimulator, ILogger<XboxControllerBinder> logger)
        {
            InitCaches();

            _controller = controller;
            _inputSimulator = inputSimulator;
            _logger = logger;

            // Instantiate services from configurations
            _leftJoyStickService = new JoyStickService(controller, inputSimulator, JoyStick.Left, logger);
            _rightJoyStickService = new JoyStickService(controller, inputSimulator, JoyStick.Right, logger);
            _buttonsService = new ButtonsService(controller, inputSimulator, ConfigCache.Configurations.Buttons, ConfigCache.Configurations.Log);

            // Instantiate handlers
            var leftJoyStickHandler = new JoyStickHandler(inputSimulator, logger);
            var rightJoyStickHandler = new JoyStickHandler(inputSimulator, logger);

            // Subscribe handlers to joystick services
            leftJoyStickHandler.Subscribe((JoyStickService)_leftJoyStickService);
            rightJoyStickHandler.Subscribe((JoyStickService)_rightJoyStickService);
        }

        private static void InitCaches()
        {
            ConfigCache.Init();
            QuadrantCache.Init();
        }

        private async Task ManageConnection()
        {
            if (ConfigCache.Configurations.LeftJoyStick.On)
            {
                _leftJoyStickService.Start();
            }

            if (ConfigCache.Configurations.RightJoyStick.On)
            {
                _rightJoyStickService.Start();
            }

            if (ConfigCache.Configurations.Buttons.On)
            {
                _buttonsService.Start();
            }
        }

        public async Task Start()
        {
            await ManageConnection();
            await Task.Run(Console.ReadLine);
        }
    }
}