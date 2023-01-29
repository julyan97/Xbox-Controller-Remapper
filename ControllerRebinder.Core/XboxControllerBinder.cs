using ControllerRebinder.Common.Enumerations;
using ControllerRebinder.Common.Moddels;
using ControllerRebinder.Common.Moddels.Configurations;
using ControllerRebinder.Common.Moddels.Configurations.SubModelsOfConfigurations;
using ControllerRebinder.Core.Caches;
using ControllerRebinder.Core.Helpers;
using ControllerRebinder.Core.Services;
using ControllerRebinder.Core.Services.Beta;
using DXNET;
using DXNET.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace ControllerRebinder.Core
{
    public class XboxControllerBinder
    {
        private Controller _controller;
        private InputSimulator _inputSimulator;
        private JoyStickService _LeftjoyStickService;
        private JoyStickService _RightjoyStickService;
        private ButtonsService _ButtonsService;

        public XboxControllerBinder()
        {
            InitCaches();
            _controller = new Controller(UserIndex.One);
            _inputSimulator = new InputSimulator();

            _LeftjoyStickService = new JoyStickService(
                _controller,
                _inputSimulator,
                ConfigCache.Configurations.LeftJoyStick,
                JoyStick.Left,
                ConfigCache.Configurations.LeftJoyStick.Log);

            _RightjoyStickService = new JoyStickService(
                _controller,
                _inputSimulator,
                ConfigCache.Configurations.RightJoyStick,
                JoyStick.Right,
                ConfigCache.Configurations.RightJoyStick.Log);

            _ButtonsService = new ButtonsService(
                _controller, _inputSimulator,
                ConfigCache.Configurations.Buttons,
                ConfigCache.Configurations.Buttons.Log);
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

