using ControllerRebinder.Common.Moddels.Configurations.SubModelsOfConfigurations;
using ControllerRebinder.Core.Services.Imp;
using DXNET.XInput;
using System;
using System.Threading.Tasks;
using WindowsInput;

namespace ControllerRebinder.Core.Services
{
    public class ButtonsService : IButtonsService
    {
        private Controller _controller;
        private InputSimulator _inputSimulator;
        private Buttons _configurations;
        private bool _log;

        public ButtonsService(
            Controller controller,
            InputSimulator inputSimulator,
            Buttons configurations,
            bool log)
        {
            _controller = controller;
            _inputSimulator = inputSimulator;
            _configurations = configurations;
            _log = log;
        }

        public async Task Start()
        {
            while(true)
            {
                var state = _controller.GetState();
                var button = state.Gamepad.Buttons;
                if(_log)
                {
                    Console.WriteLine(button);
                }

                await Run(button);

                await Task.Delay(10);
            }
        }

        private async Task Run(GamepadButtonFlags button)
        {
            if(button == GamepadButtonFlags.X)
            {
                _inputSimulator.Keyboard.KeyPress(_configurations.X);
            }
            else if(button == GamepadButtonFlags.Y)
            {
                _inputSimulator.Keyboard.KeyPress(_configurations.Y);
            }
            else if(button == GamepadButtonFlags.B)
            {
                _inputSimulator.Keyboard.KeyPress(_configurations.B);
            }
            else if(button == GamepadButtonFlags.A)
            {
                _inputSimulator.Keyboard.KeyPress(_configurations.A);
            }

            else if(button == GamepadButtonFlags.DPadUp)
            {
                _inputSimulator.Keyboard.KeyPress(_configurations.DPadUp);
            }
            else if(button == GamepadButtonFlags.DPadDown)
            {
                _inputSimulator.Keyboard.KeyPress(_configurations.DPadDown);
            }
            else if(button == GamepadButtonFlags.DPadLeft)
            {
                _inputSimulator.Keyboard.KeyPress(_configurations.DPadLeft);
            }
            else if(button == GamepadButtonFlags.DPadRight)
            {
                _inputSimulator.Keyboard.KeyPress(_configurations.DPadRight);
            }
        }
    }
}
