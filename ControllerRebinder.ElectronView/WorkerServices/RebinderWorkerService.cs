using ControllerRebinder.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ControllerRebinder.ElectronView.WorkerServices
{
    public class RebinderWorkerService : BackgroundService
    {
        private readonly ILogger<RebinderWorkerService> _logger;
        public RebinderWorkerService(ILogger<RebinderWorkerService> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
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

                await Task.Delay(10, stoppingToken);
            }
        }
    }
}
