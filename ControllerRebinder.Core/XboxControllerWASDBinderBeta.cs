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
using Range = ControllerRebinder.Core.Caches.Range;

namespace ControllerRebinder.Core
{
    public class XboxControllerWASDBinderBeta
    {
        private Controller _controller;
        private State _previousState;
        private InputSimulator _inputSimulator;

        private Quadrant _currentQuadrant = Quadrant.TopLeft;
        private Quadrant _prevQuadrant = Quadrant.TopLeft;

        private Range _currentZone;
        private Range _prevZone;
        private bool InitZones = true;

        private bool _didZoneChange = false;
        private bool _didQuadrantChange = false;

        private const int _maxValController = 32_767;
        private const double TopDownRange = 256639706.25628203;
        private const double LeftRightRange = 206639706.25628203;
        private const int DeadZone = 21815;

        public XboxControllerWASDBinderBeta()
        {
            _controller = new Controller(UserIndex.One);
            _inputSimulator = new InputSimulator();
        }

        public async Task BindStickToWASD(int threshold = 21_815)
        {
            QuadrantCache.Init();

            while(true)
            {
                Console.WriteLine($"Quadrantrs:  {_currentQuadrant} : {_prevQuadrant}");
                var state = _controller.GetState();
                var leftStickX = state.Gamepad.LeftThumbX;
                var leftStickY = state.Gamepad.LeftThumbY;


                await Run2(threshold, leftStickX, leftStickY);

                await Task.Delay(10);

            }
        }

        private async Task Run2(int threshold, int leftStickX, int leftStickY)
        {
            double StaticYAngle, StaticYArea, currentXArea;

            ExtractCurrentAndStaticAreaOfStick(threshold,
                leftStickX,
                leftStickY,
                out StaticYAngle,
                out StaticYArea,
                out currentXArea);//use to determin position in the quadrant



            List<Range> zones = QuadrantCache.Quadrants[_currentQuadrant];
            if(InitZones)
            {
                var temp = QuadrantHelper.WhereAmI(zones, currentXArea);
                _prevZone = temp;
                _currentZone = temp;
                InitZones = false;
            }


            Console.WriteLine($"Zones:  {_currentZone.Left},{_currentZone.Right} :  {_prevZone.Left},{_prevZone.Right}");

            if(Math.Abs(leftStickX) <= DeadZone && Math.Abs(leftStickY) <= DeadZone)
            {
                await ReleaseButtons(_prevZone.Buttons);
                Console.WriteLine(true);
            }
            else
            {
                if(_didQuadrantChange || _didZoneChange)
                {
                    await ReleaseButtons(_prevZone.Buttons);
                    _didZoneChange = false;
                    _didQuadrantChange = false;
                }
                else
                {
                    await PressButtons(_currentZone.Buttons);
                }
            }
            //QuadrantChange ZoneChange
            DetectQuadrantChabge(leftStickX, leftStickY);
            DetectZoneChange(currentXArea, zones);
        }

        private void DetectZoneChange(double currentXArea, List<Range> zones)
        {
            var tempZone = QuadrantHelper.WhereAmI(zones, currentXArea);
            if(_currentZone != null && tempZone != _currentZone)
            {
                _prevZone = _currentZone;
                _currentZone = tempZone;
                _didZoneChange = true;
            }
        }

        private void DetectQuadrantChabge(int leftStickX, int leftStickY)
        {
            var tempQuadrant = QuadrantHelper.WhereAmI(leftStickX, leftStickY);
            if(tempQuadrant != _currentQuadrant)
            {
                _prevQuadrant = _currentQuadrant;
                _currentQuadrant = tempQuadrant;
                _didQuadrantChange = true;
            }
        }

        private async Task ReleaseKeys()
        {
            foreach(var ranges in QuadrantCache.Quadrants.Values)
            {
                foreach(var range in ranges)
                {
                    foreach(var button in range.Buttons)
                    {
                        await Task.Run(() => _inputSimulator.Keyboard.KeyUp(button));
                    }
                }
            }
        }

        private async Task PressButtons(List<VirtualKeyCode> buttons)
        {
            foreach(var button in buttons)
            {
                await Task.Run(() => _inputSimulator.Keyboard.KeyDown(button));
            }
        }

        private async Task ReleaseButtons(List<VirtualKeyCode> buttons)
        {
            foreach(var button in buttons)
            {
                await Task.Run(() => _inputSimulator.Keyboard.KeyUp(button));
            }
        }

        private void Run(int threshold, int leftStickX, int leftStickY)
        {
            double StaticYAngle, StaticYArea, currentArea;

            ExtractCurrentAndStaticAreaOfStick(threshold,
                leftStickX,
                leftStickY,
                out StaticYAngle,
                out StaticYArea,
                out currentArea);

            Console.WriteLine($"{leftStickX} : {leftStickY}");
            Console.WriteLine(StaticYAngle); // 45

            Console.WriteLine(StaticYArea); // 186639706.25628203
            Console.WriteLine(currentArea); // MyArea

            //Console.WriteLine(area2);

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

                if(currentArea > StaticYArea)
                {
                    if(leftStickY > 0)
                    {
                        if(currentArea > TopDownRange)
                            _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_W);

                        if(currentArea < TopDownRange && currentArea < LeftRightRange && leftStickX < 0)
                        {
                            _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);

                        }
                        if(currentArea < TopDownRange && currentArea < LeftRightRange && leftStickX > 0)
                        {
                            _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                        }
                    }
                    else
                    {
                        _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                        _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                        _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                    }

                    if(leftStickY < 0)
                    {
                        if(currentArea > TopDownRange)
                            _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_S);

                        if(currentArea < TopDownRange && leftStickX < 0)
                        {
                            _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);

                        }
                        if(currentArea < TopDownRange && leftStickX > 0)
                        {
                            _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);

                        }

                    }
                    else
                    {
                        _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_S);
                        _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                        _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                    }
                }
                else if(currentArea < StaticYArea)
                {
                    if(leftStickX > 0 && currentArea < LeftRightRange)
                    {
                        _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                    }
                    else
                    {
                        _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                    }

                    if(leftStickX < 0 && currentArea < LeftRightRange)
                    {
                        _inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                    }
                    else
                    {
                        _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                    }

                }
                else
                {
                    _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                    _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                    _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_S);
                    _inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                }

            }



        }

        private void ExtractCurrentAndStaticAreaOfStick(int threshold, int leftStickX, int leftStickY, out double StaticYAngle, out double StaticYArea, out double currentXArea)
        {

            FimdArea(threshold, _maxValController, _maxValController, out StaticYAngle, out StaticYArea);
            FimdArea(threshold, Math.Abs(leftStickX), Math.Abs(leftStickY), out double angle1, out currentXArea);
            FimdArea(threshold, Math.Abs(leftStickY), Math.Abs(leftStickX), out double angle2, out double currentYArea);


            Console.WriteLine(_currentQuadrant);
            Console.WriteLine($"X (left-right):{leftStickX} : Y (up-down):{leftStickY}");

            Console.WriteLine($"static:{StaticYArea} : X:{currentXArea} : Y:{currentYArea}"); // 186639706.25628203
            Console.WriteLine();
        }

        private static void FimdArea(int threshold, int leftStickX, int leftStickY, out double angle, out double area)
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

