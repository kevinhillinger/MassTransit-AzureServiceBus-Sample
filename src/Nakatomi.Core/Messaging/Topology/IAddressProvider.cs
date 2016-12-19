using System;

namespace Nakatomi.Messaging.Topology
{
    interface IAddressProvider
    {
        void Register(string queueName);
        void Register(Type messageType);
        void Register<TMessage>();
        void Register(Type messageType, Uri address);
        Uri GetAddress<TMessage>();
        Uri GetAddress(string queueName);

        string GetQueueName(Type messageType);
    }
}
