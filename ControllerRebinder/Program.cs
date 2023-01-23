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
            Start:
            try
            {
                var controllerRebinder = new XboxControllerBinder();
                await controllerRebinder.Start();
            }
            catch (Exception ex) {
            goto Start;
            }

        }
    }
}

