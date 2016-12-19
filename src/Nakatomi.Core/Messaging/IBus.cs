using System.Threading;
using System.Threading.Tasks;

namespace Nakatomi.Messaging
{
    public interface IBus : ISendMessages
    {
        string Name { get; }
        Task Start(CancellationToken cancellationToken = default(CancellationToken));
        Task Stop(CancellationToken cancellationToken = default(CancellationToken));
    }
}
