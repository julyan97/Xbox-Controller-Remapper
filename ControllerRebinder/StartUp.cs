using ControllerRebinder.Core;
using ControllerRebinder.TesterConsole.HostedServices;
using DXNET.XInput;
using Microsoft.Extensions.DependencyInjection;
using WindowsInput;

namespace ControllerRebinder.TesterConsole
{
    public static class StartUp
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services
                   .AddSingleton<Controller>(new Controller(UserIndex.One))
                   .AddSingleton<InputSimulator>()
                   .AddTransient<XboxControllerBinder>()
                   .AddHostedService<XboxControllerBinderService>(); 

        }
    }
}
