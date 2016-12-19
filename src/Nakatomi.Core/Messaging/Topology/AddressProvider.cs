using System;
using System.Collections.Generic;

namespace Nakatomi.Messaging.Topology
{
    class AddressProvider : IAddressProvider
    {
        Dictionary<string, Uri> addresses = new Dictionary<string, Uri>();
        private Uri baseUri;

        public AddressProvider(Uri baseUri)
        {
            this.baseUri = baseUri;
        }

        public void Register<TMessage>()
        {
            Register(typeof(TMessage));
        }

        public void Register(string queueName)
        {
            addresses.Add(queueName, new Uri(baseUri, queueName));
        }

        public void Register(Type messageType)
        {
            var queueName = GetQueueName(messageType);
            Register(messageType, new Uri(baseUri, queueName));
        }

        public void Register(Type messageType, Uri inputAddress)
        {
            addresses.Add(GetQueueName(messageType), inputAddress);
        }

        public Uri GetAddress<TMessage>()
        {
            var queueName = GetQueueName(typeof(TMessage));
            return GetAddress(queueName);
        }

        public Uri GetAddress(string queueName)
        {
            Uri address;

            if (!addresses.TryGetValue(queueName, out address))
            {
                throw new InvalidOperationException($"No address defined for queue '{queueName}'.");
            }

            return address;
        }

        public string GetQueueName(Type messageType)
        {
            return messageType.FullName.ToLower();
        }
    }
}
