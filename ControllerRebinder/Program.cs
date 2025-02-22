using System;
using System.Threading;
using System.Threading.Tasks;
using ControllerRebinder.Core;
using ControllerRebinder.Core.Caches;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ControllerRebinder.TesterConsole;

public class Program
{
    static async Task Main(string[] args)
    {
        // Initialize configuration
        try
        {
            await ConfigCache.InitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading configuration: {ex.Message}");
            return;
        }
    
        var host = CreateHostBuilder(args).Build();
        var serviceProvider = host.Services;

        // Use cancellation token for graceful shutdown
        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (s, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        // Use GetRequiredService to ensure the service is available
        var controllerRebinder = serviceProvider.GetRequiredService<XboxControllerBinder>();

        try
        {
            await controllerRebinder.Start(cts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Expected on cancellation; log or handle as needed
        }
    }


    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) => { services.ConfigureServices(); }).ConfigureLogging((_, logging) =>
            {
                if (ConfigCache.Configurations.Log)
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                }
            });
}