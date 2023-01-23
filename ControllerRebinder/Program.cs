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
            var controllerRebinder = new XboxControllerBinder();
            await controllerRebinder.Start();

        }
    }
}

