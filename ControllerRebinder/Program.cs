using ControllerRebinder.Core;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WindowsInput;
using DXNET.XInput;

namespace ControllerRebinder
{
    public class Program
    {
        static async Task Main(string[] args)
        {

            var serviceProvider = new ServiceCollection()
                .AddSingleton<Controller>(new Controller(UserIndex.One))
                .AddSingleton<InputSimulator>()
                .AddTransient<XboxControllerBinder>()
                .BuildServiceProvider();

            Start:
            try
            {
                var controllerRebinder = serviceProvider.GetService<XboxControllerBinder>();
                await controllerRebinder.Start();
            }
            catch(Exception ex)
            {
                goto Start;
            }

        }
    }
}

