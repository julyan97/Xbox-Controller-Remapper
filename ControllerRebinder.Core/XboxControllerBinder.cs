using DXNET.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;
using WindowsInput;
using ControllerRebinder.Core.Helpers;
using ControllerRebinder.Core.Enumerations;
using ControllerRebinder.Core.Caches;
using ControllerRebinder.Common.Moddels;
using ControllerRebinder.Common.Moddels.Configurations;

namespace ControllerRebinder.Core
{
    public class XboxControllerBinder
    {
        private Controller _controller;
        private InputSimulator _inputSimulator;
        private Configurations _configuration;

        private Quadrant _currentQuadrant = Quadrant.TopLeft;
        private Quadrant _prevQuadrant = Quadrant.TopLeft;

        private ZoneRange _currentZone;
        private ZoneRange _prevZone;
        private bool InitZones = true;

        private bool _didZoneChange = false;
        private bool _didQuadrantChange = false;


        public XboxControllerBinder()
        {
            ConfigCache.Init();
            QuadrantCache.Init();
            
            _controller = new Controller(UserIndex.One);
            _inputSimulator = new InputSimulator();

            _configuration = ConfigCache.Configurations;
        }

        public async Task Start()
        {
            while(true)
            {
                Console.WriteLine($"Quadrantrs:  {_currentQuadrant} : {_prevQuadrant}");
                var state = _controller.GetState();
                var leftStickX = state.Gamepad.LeftThumbX;
                var leftStickY = state.Gamepad.LeftThumbY;

                await Run(_configuration.LeftJoyStick.DeadZone, leftStickX, leftStickY);

                await Task.Delay(10);

            }
        }

        private async Task Run(int threshold, int leftStickX, int leftStickY)
        {
            double StaticYAngle, StaticYArea, currentXArea;

            ExtractCurrentAndStaticAreaOfStick(threshold,
                leftStickX,
                leftStickY,
                out StaticYAngle,
                out StaticYArea,
                out currentXArea);//use to determin position in the quadrant

            List<ZoneRange> zones = InitCurrentZonezForQuadrant(currentXArea);

            Console.WriteLine($"Zones:  {_currentZone.Left},{_currentZone.Right} :  {_prevZone.Left},{_prevZone.Right}");

            if(Math.Abs(leftStickX) <= _configuration.LeftJoyStick.DeadZone && Math.Abs(leftStickY) <= _configuration.LeftJoyStick.DeadZone)
            {
                await ButtonHelper.ReleaseButtons(_prevZone.Buttons);
                Console.WriteLine(true);
            }
            else
            {
                if(_didQuadrantChange || _didZoneChange)
                {
                    var toРeslease = _prevZone.Buttons.Where(x => !_currentZone.Buttons.Contains(x)).ToList();
                    await ButtonHelper.ReleaseButtons(toРeslease);
                    _didZoneChange = false;
                    _didQuadrantChange = false;
                }
                else
                {
                    await ButtonHelper.PressButtons(_currentZone.Buttons);
                }
            }
            //QuadrantChange ZoneChange
            DetectQuadrantChange(leftStickX, leftStickY);
            DetectZoneChange(currentXArea, zones);
        }

        private List<ZoneRange> InitCurrentZonezForQuadrant(double currentXArea)
        {
            List<ZoneRange> zones = QuadrantCache.Quadrants[_currentQuadrant];
            if(InitZones)
            {
                var temp = QuadrantHelper.WhereAmI(zones, currentXArea);
                _prevZone = temp;
                _currentZone = temp;
                InitZones = false;
            }

            return zones;
        }

        /// <summary>
        /// Keep track of the previous and current zone and if it changes _didZoneChange will become true 
        /// </summary>
        private void DetectZoneChange(double currentXArea, List<ZoneRange> zones)
        {
            var tempZone = QuadrantHelper.WhereAmI(zones, currentXArea);
            if(_currentZone != null && tempZone != _currentZone)
            {
                _prevZone = _currentZone;
                _currentZone = tempZone;
                _didZoneChange = true;
            }
        }

        /// <summary>
        /// Keep track of the previous and current zone and if it changes _didQuadrantChange will become true 
        /// </summary>
        private void DetectQuadrantChange(int leftStickX, int leftStickY)
        {
            var tempQuadrant = QuadrantHelper.WhereAmI(leftStickX, leftStickY);
            if(tempQuadrant != _currentQuadrant)
            {
                _prevQuadrant = _currentQuadrant;
                _currentQuadrant = tempQuadrant;
                _didQuadrantChange = true;
            }
        }

        private void ExtractCurrentAndStaticAreaOfStick(int threshold, int leftStickX, int leftStickY, out double StaticYAngle, out double StaticYArea, out double currentXArea)
        {

            FiнdArea(threshold, _configuration.LeftJoyStick.MaxValController, _configuration.LeftJoyStick.MaxValController, out StaticYAngle, out StaticYArea);
            FiнdArea(threshold, Math.Abs(leftStickX), Math.Abs(leftStickY), out double CurrenrtAngle, out currentXArea);


            Console.WriteLine(_currentQuadrant);
            Console.WriteLine($"X (left-right):{leftStickX} : Y (up-down):{leftStickY}");

            Console.WriteLine($"static:{StaticYArea} : X:{currentXArea}"); // 186639706.25628203
            Console.WriteLine();
        }

        private static void FiнdArea(int threshold, int leftStickX, int leftStickY, out double angle, out double area)
        {
            angle = Math.Atan2(leftStickY, leftStickX);
            angle = angle * (180 / Math.PI);
            area = (angle / 360) * Math.PI * Math.Pow(threshold, 2);
        }
        public async Task Original(int threshold = 21_815)
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
    }
}

