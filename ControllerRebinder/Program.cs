using ControllerRebinder.Core;
using ControllerRebinder.TesterConsole;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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
                services.ConfigureServices();

            }).ConfigureLogging((_, logging) =>
            {
                logging.ClearProviders();
                logging.AddSimpleConsole(options => options.IncludeScopes = true);
            });



    }
}
