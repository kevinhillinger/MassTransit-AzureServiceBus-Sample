using System;
using System.Threading;
using System.Threading.Tasks;

namespace Nakatomi.Messaging
{
    /// <summary>
    /// Sends messages
    /// </summary>
    public interface ISendMessages
    {
        Task Send<T>(T message, CancellationToken cancellationToken = default(CancellationToken)) where T : class;
        Task Send<T>(object message, CancellationToken cancellationToken = default(CancellationToken)) where T : class;
    }
}
