using DXNET.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace ControllerRebinder.Core
{

    public class XboxControllerWASDBinder
    {
        private Controller _controller;
        private State _previousState;
        private InputSimulator _inputSimulator;
        private const int _maxValController = 32_767;
        private int _horizontalRange;
        private int _verticalRange;
        private const int DeadZone = 21_815;

        public XboxControllerWASDBinder()
        {
            _controller = new Controller(UserIndex.One);
            _inputSimulator = new InputSimulator();
        }

        public async Task BindStickToWASD(int horizontalRange, int verticalRange, int threshold = 21_815)
        {
            _horizontalRange = horizontalRange;
            _verticalRange = verticalRange;

            while(true)
            {
                var state = _controller.GetState();
                var leftStickX = state.Gamepad.LeftThumbX;
                var leftStickY = state.Gamepad.LeftThumbY;
                Console.WriteLine($"{leftStickX} : {leftStickY}");
                // Check left stick movement on the X axis
                RemapStickToWASD(leftStickX, leftStickY);

                await Task.Delay(10);
            }
        }

        private void RemapStickToWASD(int leftStickX, int leftStickY)
        {
            if(Math.Abs(leftStickX) <= DeadZone && Math.Abs(leftStickY) <= DeadZone)
            {
                Console.WriteLine(true);

                _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_S);
                _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
            }
            else
            {
                // Check left stick movement on the X axis
                if(Math.Abs(leftStickX) > DeadZone && Math.Abs(leftStickX) > _horizontalRange)
                {
                    if(leftStickX > 0)
                    {
                        _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                    }
                    else
                    {
                        _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                    }
                }
                else
                {
                    _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                    _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                }
                // Check left stick movement on the Y axis
                if(Math.Abs(leftStickY) > DeadZone && Math.Abs(leftStickY) > _verticalRange)
                {
                    if(leftStickY > 0)
                    {
                        _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_W);
                    }
                    else
                    {
                        _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_S);
                    }
                }
                else
                {
                    _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                    _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_S);
                }
            }
        }

        public void RemapStickToWASD(float deadZone, float horizontalRange, float verticalRange)
        {
            // Get the current state of the controller
            _previousState = _controller.GetState();
            // Check if the left stick is being moved
            float x = _previousState.Gamepad.LeftThumbX;
            float y = _previousState.Gamepad.LeftThumbY;
            Console.WriteLine($"{x} : {y}");
            // Check if the left stick is within the dead zone
            if(Math.Abs(x) < deadZone && Math.Abs(y) < deadZone)
            {
                // If the left stick is within the dead zone, release all keys
                _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_S);
            }
            else
            {
                // Check the horizontal and vertical ranges to determine key presses
                if(Math.Abs(x) > horizontalRange)
                {
                    if(x < -deadZone)
                    {
                        _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                    }
                    else if(x > deadZone)
                    {
                        _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                    }
                }

                if(Math.Abs(y) > verticalRange)
                {
                    if(y < -deadZone)
                    {
                        _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_W);
                    }
                    else if(y > deadZone)
                    {
                        _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_S);
                    }
                }
            }

        }
    }
}
