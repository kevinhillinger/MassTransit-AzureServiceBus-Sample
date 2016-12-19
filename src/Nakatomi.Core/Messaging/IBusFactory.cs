using Nakatomi.Messaging.Configuration;

namespace Nakatomi.Messaging
{
    public interface IBusFactory
    {
        IBus Create(BusConfiguration configuration);
    }
}
