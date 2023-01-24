using ControllerRebinder.Core;
using System;
using System.Threading.Tasks;

namespace ControllerRebinder
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
            catch(Exception ex)
            {
                goto Start;
            }

        }
    }
}

