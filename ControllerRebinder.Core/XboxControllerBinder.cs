using ControllerRebinder.Common.Enumerations;
using ControllerRebinder.Core.Caches;
using ControllerRebinder.Core.Events.Versions.v04;
using ControllerRebinder.Core.Services;
using ControllerRebinder.Core.Services.Imp;
using DXNET.XInput;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
            _leftJoyStickService = new JoyStickService_v04(controller, inputSimulator, JoyStick.Left, logger);
            _rightJoyStickService = new JoyStickService_v04(controller, inputSimulator, JoyStick.Right, logger);
            _buttonsService = new ButtonsService(controller, inputSimulator, ConfigCache.Configurations.Buttons, ConfigCache.Configurations.Log);

            // Instantiate handlers
            var leftJoyStickHandler = new JoyStickHandler_v04(inputSimulator, logger);
            var rightJoyStickHandler = new JoyStickHandler_v04(inputSimulator, logger);

            // Subscribe handlers to joystick services
            leftJoyStickHandler.Subscribe((JoyStickService_v04)_leftJoyStickService);
            rightJoyStickHandler.Subscribe((JoyStickService_v04)_rightJoyStickService);
        }

        private static void InitCaches()
        {
            ConfigCache.Init();
            QuadrantCache.Init();
        }

        private async Task ManageConnection()
        {
            var tasks = new List<Task>();

            if (ConfigCache.Configurations.LeftJoyStick.On)
            {
                tasks.Add(_leftJoyStickService.Start());
            }

            if (ConfigCache.Configurations.RightJoyStick.On)
            {
                tasks.Add(_rightJoyStickService.Start());
            }

            if (ConfigCache.Configurations.Buttons.On)
            {
                tasks.Add(_buttonsService.Start());
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        public async Task Start()
        {
            await ManageConnection();
            Console.ReadLine();
        }
    }
}
