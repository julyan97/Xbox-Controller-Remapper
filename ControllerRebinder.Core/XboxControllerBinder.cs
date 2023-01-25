using ControllerRebinder.Common.Enumerations;
using ControllerRebinder.Common.Moddels;
using ControllerRebinder.Common.Moddels.Configurations;
using ControllerRebinder.Common.Moddels.Configurations.SubModelsOfConfigurations;
using ControllerRebinder.Core.Caches;
using ControllerRebinder.Core.Helpers;
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
        private bool ReleaseButtons = true;
        private Configurations _configuration;
        private List<VirtualKeyCode> releasedButtons;

        private Quadrant _currentQuadrant = Quadrant.TopLeft;
        private Quadrant _prevQuadrant = Quadrant.TopLeft;

        private ZoneRange _currentZone;
        private ZoneRange _prevZone;
        private bool InitZones = true;

        private bool _didZoneChange = false;
        private bool _didQuadrantChange = false;

        private double StaticYArea;
        private double _currentXArea;
        public XboxControllerBinder()
        {
            InitCaches();

            _controller = new Controller(UserIndex.One);
            _inputSimulator = new InputSimulator();
            releasedButtons = new List<VirtualKeyCode>();
            _configuration = ConfigCache.Configurations;
        }

        private void InitCaches()
        {
            ConfigCache.Init();
            QuadrantCache.Init();
        }

        public async Task Start()
        {
            CircleHelper.FindArea(_configuration.LeftJoyStick.ThreshHoldAreaCal, _configuration.LeftJoyStick.MaxValController, _configuration.LeftJoyStick.MaxValController, out double StaticYAngle, out StaticYArea);
            while(true)
            {
                var state = _controller.GetState();
                var leftStickX = state.Gamepad.LeftThumbX;
                var leftStickY = state.Gamepad.LeftThumbY;

                await Run_3_0(leftStickX, leftStickY);

                await Task.Delay(10);

            }
        }

        private async Task Run_3_0(int leftStickX, int leftStickY)
        {
            var upDown = ConfigCache.Configurations.LeftJoyStick.ForwardDown;
            var leftRight = ConfigCache.Configurations.LeftJoyStick.LeftRight;
            var controlls = ConfigCache.Configurations.LeftJoyStick.Controlls;
            var deadZone = ConfigCache.Configurations.LeftJoyStick.DeadZone;
            var keyboard = _inputSimulator.Keyboard;

            Console.WriteLine($"X (left-right):{leftStickX} : Y (up-down):{leftStickY}\nstatic:{this.StaticYArea} : X:{_currentXArea}\n");

            if(CircleHelper.isInDeadZone(leftStickX, leftStickY, deadZone))
            {
                await controlls.ReleaseAll(_inputSimulator);

            }
            else
            {
                CircleHelper.FindArea(_configuration.LeftJoyStick.ThreshHoldAreaCal, Math.Abs(leftStickX), Math.Abs(leftStickY), out double CurrenrtAngle, out _currentXArea);
                StickMovementByQuadrants(_currentXArea, upDown, leftRight, controlls, keyboard);
            }

            //QuadrantChange ZoneChange
            _currentQuadrant = QuadrantHelper.WhereAmI(leftStickX, leftStickY);
            
        }

        private async Task Run_2_0(int leftStickX, int leftStickY)
        {
            double currentXArea;
            var deadZone = ConfigCache.Configurations.LeftJoyStick.DeadZone;


            ExtractCurrentArea(
                leftStickX,
                leftStickY,
                out currentXArea);//use to determin position in the quadrant

            List<ZoneRange> zones = CircleHelper.InitCurrentZonezForQuadrant(currentXArea, ref _currentQuadrant, ref _currentZone, ref _prevZone, ref InitZones);

            Console.WriteLine($"Zones:  {_currentZone.Left},{_currentZone.Right} :  {_prevZone.Left},{_prevZone.Right}");

            if(CircleHelper.isInDeadZone(leftStickX, leftStickY, deadZone))
            {

                var shouldRelease = _prevZone.Buttons;
                if(shouldRelease != releasedButtons)
                {
                    await ButtonHelper.ReleaseButtons(shouldRelease);
                }
                releasedButtons = shouldRelease;
            }
            else if(_didQuadrantChange || _didZoneChange)
            {
                var shouldRelease = _prevZone.Buttons.Where(x => !_currentZone.Buttons.Contains(x)).ToList();
                if(shouldRelease != releasedButtons)
                {
                    await ButtonHelper.ReleaseButtons(shouldRelease);
                }
                releasedButtons = shouldRelease;
                _didZoneChange = false;
                _didQuadrantChange = false;
            }
            else
            {
                await ButtonHelper.PressButtons(_currentZone.Buttons);
                ReleaseButtons = true;
            }
            //QuadrantChange ZoneChange
            CircleHelper.DetectQuadrantChange(leftStickX, leftStickY, ref _currentQuadrant, ref _prevQuadrant, ref _didQuadrantChange);
            CircleHelper.DetectZoneChange(currentXArea, zones, ref _currentZone, ref _prevZone, ref _didZoneChange);
        }

        public async Task Run_1_0(int threshold = 21_815)
        {
            while(true)
            {
                var state = _controller.GetState();
                var leftStickX = state.Gamepad.LeftThumbX;
                var leftStickY = state.Gamepad.LeftThumbY;

                Console.WriteLine($"{leftStickX} : {leftStickY}");

                // Check left stick movement on the X axis
                if(leftStickX > threshold)
                {
                    _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                }
                else if(leftStickX < -threshold)
                {
                    _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                }
                else
                {
                    _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                    _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                }
                // Check left stick movement on the Y axis
                if(leftStickY > threshold)
                {
                    _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_W);
                }
                else if(leftStickY < -threshold)
                {
                    _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_S);
                }
                else
                {
                    _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                    _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_S);
                }


                await Task.Delay(10);
            }
        }

        private void ExtractCurrentArea(int leftStickX, int leftStickY, out double currentXArea)
        {
            CircleHelper.FindArea(_configuration.LeftJoyStick.ThreshHoldAreaCal, Math.Abs(leftStickX), Math.Abs(leftStickY), out double CurrenrtAngle, out currentXArea);
        }

        private void StickMovementByQuadrants(double currentXArea, double upDown, double leftRight, Controlls controlls, IKeyboardSimulator keyboard)
        {
            if(_currentQuadrant == Quadrant.TopLeft && (currentXArea > leftRight && currentXArea < upDown))
            {
                keyboard.KeyDown(controlls.Up);
                keyboard.KeyDown(controlls.Left);
            }
            else if(_currentQuadrant == Quadrant.TopRight && (currentXArea > leftRight && currentXArea < upDown))
            {
                keyboard.KeyDown(controlls.Up);
                keyboard.KeyDown(controlls.Right);
            }
            else if(_currentQuadrant == Quadrant.BottomLeft && (currentXArea > leftRight && currentXArea < upDown))
            {
                keyboard.KeyDown(controlls.Down);
                keyboard.KeyDown(controlls.Left);
            }
            else if(_currentQuadrant == Quadrant.BottomRight && (currentXArea > leftRight && currentXArea < upDown))
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

