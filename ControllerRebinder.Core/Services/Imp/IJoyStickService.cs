using System.Threading;
using System.Threading.Tasks;

namespace ControllerRebinder.Core.Services.Imp
{
    public interface IJoyStickService
    {
        Task Start(CancellationToken cancellationToken = default);
    }
}
