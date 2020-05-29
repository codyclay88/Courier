using System.Threading;
using System.Threading.Tasks;

namespace Courier
{
    public interface ICourierReceiver
    {
        Task StartListening(CancellationToken cancellationToken);
        Task StopListening(CancellationToken cancellationToken);
    }
}