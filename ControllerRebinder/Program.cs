using ControllerRebinder.Core;
using DXNET.XInput;
using System;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace ConsoleApp2
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var controller = new XboxControllerWASDBinderBeta();

            Start:
            try
            {
                await controller.BindStickToWASD();
            }
            catch
            {
                goto Start;
            }




        }
    }






    public class XboxControllerWASDBinder2
    {
        private Controller _controller;
        private State _previousState;
        private InputSimulator _inputSimulator;
        private const int _maxValController = 32_767;
        private const double TopDownRange = 256639706.25628203;
        private const double LeftRightRange = 206639706.25628203;
        private const int DeadZone = 21815;

        public XboxControllerWASDBinder2()
        {
            _controller = new Controller(UserIndex.One);
            _inputSimulator = new InputSimulator();
        }

        public async Task BindStickToWASD(int threshold = 21_815, int xSensitivity = 21_688, int ySensitivity = 19_457)
        {
            while(true)
            {
                var state = _controller.GetState();
                var leftStickX = state.Gamepad.LeftThumbX;
                var leftStickY = state.Gamepad.LeftThumbY;

                // Check left stick movement on the X axis
                Run(threshold, leftStickX, leftStickY);

                await Task.Delay(10);
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

        private static void ExtractCurrentAndStaticAreaOfStick(int threshold, int leftStickX, int leftStickY, out double StaticYAngle, out double StaticYArea, out double currentArea)
        {

            FimdArea(threshold, _maxValController, _maxValController, out StaticYAngle, out StaticYArea);
            FimdArea(threshold, Math.Abs(leftStickX), Math.Abs(leftStickY), out double angle1, out currentArea);


            FimdArea(threshold, 18_000, 18_000, out double StaticXAngle, out double StaticXArea);
            FimdArea(threshold, Math.Abs(leftStickY), Math.Abs(leftStickX), out double angle2, out double area2);
        }

        private static void FimdArea(int threshold, int leftStickX, int leftStickY, out double angle, out double area)
        {
            angle = Math.Atan2(leftStickY, leftStickX);
            angle = angle * (180 / Math.PI);
            area = (angle / 360) * Math.PI * Math.Pow(threshold, 2);
        }
    }
}

