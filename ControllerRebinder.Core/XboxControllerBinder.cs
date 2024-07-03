using ControllerRebinder.Common.Enumerations;
using ControllerRebinder.Core.Caches;
using ControllerRebinder.Core.Events;
using ControllerRebinder.Core.Services;
using ControllerRebinder.Core.Services.Imp;// Add this line to include the handler namespace
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
        private readonly JoyStickHandler _leftJoyStickHandler;  
        private readonly JoyStickHandler _rightJoyStickHandler; 

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
            _leftJoyStickHandler = new JoyStickHandler(inputSimulator, logger);
            _rightJoyStickHandler = new JoyStickHandler(inputSimulator, logger);

            // Subscribe handlers to joystick services
            _leftJoyStickHandler.Subscribe((JoyStickService)_leftJoyStickService);
            _rightJoyStickHandler.Subscribe((JoyStickService)_rightJoyStickService);
        }

        private void InitCaches()
        {
            ConfigCache.Init();
            QuadrantCache.Init();
        }

        public async Task Start()
        {
            if (ConfigCache.Configurations.LeftJoyStick.On)
            {
                _ = _leftJoyStickService.Start();
            }
            if (ConfigCache.Configurations.RightJoyStick.On)
            {
                _ = _rightJoyStickService.Start();
            }
            if (ConfigCache.Configurations.Buttons.On)
            {
                _ = _buttonsService.Start();
            }

            await Task.Run(() => Console.ReadLine());
        }
    }
}
