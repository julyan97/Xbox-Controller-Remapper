using ControllerRebinder.Core;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WindowsInput;
using DXNET.XInput;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace ControllerRebinder
{
    public class Program
    {
        static async Task Main(string[] args)
        {

            var serviceProvider = CreateHostBuilder(args).Build();

            Start:
            try
            {
                var controllerRebinder = serviceProvider.Services.GetService<XboxControllerBinder>();
                await controllerRebinder.Start();
            }
            catch(Exception ex)
            {
                goto Start;
            }

        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<Controller>(new Controller(UserIndex.One))
                        .AddSingleton<InputSimulator>()
                        .AddTransient<XboxControllerBinder>();

            }).ConfigureLogging((_, logging) =>
            {
                logging.ClearProviders();
                logging.AddSimpleConsole(options => options.IncludeScopes = true);
            });
    }
}
