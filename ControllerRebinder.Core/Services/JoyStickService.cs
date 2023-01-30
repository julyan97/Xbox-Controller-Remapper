using ControllerRebinder.Common.Enumerations;
using ControllerRebinder.Common.Moddels.Configurations.SubModelsOfConfigurations;
using ControllerRebinder.Common.Moddels.Configurations;
using ControllerRebinder.Common.Moddels;
using ControllerRebinder.Core.Caches;
using ControllerRebinder.Core.Helpers;
using DXNET.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;
using WindowsInput;
using DXNET;
using ControllerRebinder.Core.Services.Imp;

namespace ControllerRebinder.Core.Services
{
    public class JoyStickService : IJoyStickService
    {
        private Controller _controller;
        private InputSimulator _inputSimulator;
        private BaseJoyStick _configuration;
        private JoyStick _joyStick;
        private bool _log;
        private Quadrant _currentQuadrant = Quadrant.TopLeft;
        private double StaticYArea;
        private double _currentXArea;

        public JoyStickService(
            Controller controller,
            InputSimulator inputSimulator,
            BaseJoyStick configurations,
            JoyStick joyStick,
            bool log)
        {
            _controller = controller;
            _inputSimulator = inputSimulator;
            _configuration = configurations;
            _joyStick = joyStick;
            _log = log;
        }

        public async Task Start()
        {
            CircleHelper.FindArea(
                _configuration.ThreshHoldAreaCal,
                _configuration.MaxValController,
                _configuration.MaxValController,
                out double StaticYAngle,
                out StaticYArea);


            while(true)
            {
                var state = _controller.GetState();
                short stickX = 0;
                short stickY = 0;

                ChooseJoyStick(state, ref stickX, ref stickY);

                await Run_3_0(stickX, stickY);

                await Task.Delay(10);
            }
        }

        private void ChooseJoyStick(State state, ref short leftStickX, ref short leftStickY)
        {
            if(_joyStick == JoyStick.Left)
            {
                leftStickX = state.Gamepad.LeftThumbX;
                leftStickY = state.Gamepad.LeftThumbY;
            }
            else if(_joyStick == JoyStick.Right)
            {
                leftStickX = state.Gamepad.RightThumbX;
                leftStickY = state.Gamepad.RightThumbY;
            }
        }

        private async Task Run_3_0(int leftStickX, int leftStickY)
        {
            var upDown = _configuration.ForwardDown;
            var leftRight = _configuration.LeftRight;
            var controlls = _configuration.Controlls;
            var deadZone = _configuration.DeadZone;
            var keyboard = _inputSimulator.Keyboard;

            if(_log)
            {
                 Log(leftStickX, leftStickY);
            }
           

            if(CircleHelper.isInDeadZone(leftStickX, leftStickY, deadZone))
            {
                await controlls.ReleaseAll(_inputSimulator);

            }
            else
            {
                CircleHelper.FindArea(
                    _configuration.ThreshHoldAreaCal,
                    Math.Abs(leftStickX), Math.Abs(leftStickY),
                    out double CurrenrtAngle,
                    out _currentXArea);

                StickMovementByQuadrants(_currentXArea, upDown, leftRight, controlls, keyboard);
            }

            //QuadrantChange ZoneChange
            _currentQuadrant = QuadrantHelper.WhereAmI(leftStickX, leftStickY);
        }

        private void Log(int leftStickX, int leftStickY)
        {
            Console.WriteLine("Version 3.0");
            Console.WriteLine($"X (left-right):{leftStickX} : Y (up-down):{leftStickY}\nstatic:{StaticYArea} : X:{_currentXArea}\n");
        }

        private void StickMovementByQuadrants(double currentXArea, double upDown, double leftRight, Controlls controlls, IKeyboardSimulator keyboard)
        {
            if(_currentQuadrant == Quadrant.TopLeft && currentXArea > leftRight && currentXArea < upDown)
            {
                keyboard.KeyDown(controlls.Up);
                keyboard.KeyDown(controlls.Left);
            }
            else if(_currentQuadrant == Quadrant.TopRight && currentXArea > leftRight && currentXArea < upDown)
            {
                keyboard.KeyDown(controlls.Up);
                keyboard.KeyDown(controlls.Right);
            }
            else if(_currentQuadrant == Quadrant.BottomLeft && currentXArea > leftRight && currentXArea < upDown)
            {
                keyboard.KeyDown(controlls.Down);
                keyboard.KeyDown(controlls.Left);
            }
            else if(_currentQuadrant == Quadrant.BottomRight && currentXArea > leftRight && currentXArea < upDown)
            {
                keyboard.KeyDown(controlls.Down);
                keyboard.KeyDown(controlls.Right);
            }
            else
            {
                keyboard.KeyUp(controlls.Right);
                keyboard.KeyUp(controlls.Up);
                keyboard.KeyUp(controlls.Left);
                keyboard.KeyUp(controlls.Down);
            }


            if((_currentQuadrant == Quadrant.TopLeft || _currentQuadrant == Quadrant.TopRight) && currentXArea > upDown)
            {
                keyboard.KeyDown(controlls.Up);
            }

            if((_currentQuadrant == Quadrant.TopLeft || _currentQuadrant == Quadrant.BottomLeft) && currentXArea < leftRight)
            {
                keyboard.KeyDown(controlls.Left);
            }

            if((_currentQuadrant == Quadrant.BottomLeft || _currentQuadrant == Quadrant.BottomRight) && currentXArea > upDown)
            {
                keyboard.KeyDown(controlls.Down);
            }

            if((_currentQuadrant == Quadrant.BottomRight || _currentQuadrant == Quadrant.TopRight) && currentXArea < leftRight)
            {
                keyboard.KeyDown(controlls.Right);
            }
        }

    }
}
