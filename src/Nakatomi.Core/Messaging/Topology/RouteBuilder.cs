
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Nakatomi.Messaging.Configuration;
using Nakatomi.Messaging.Topology;
using Nakatomi.Transactions;
using Nakatomi.Utilities;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Nakatomi.Messaging.Topology
{
    class RouteBuilder
    {
        MessageConventions conventions;

        IAssemblyScanner assemblyScanner;
        IAddressProvider addressProvider;
        IContainer container;

        public RouteBuilder(IContainer container, IAssemblyScanner assemblyScanner, IAddressProvider addressProvider, MessageConventions conventions)
        {
            this.container = container;
            this.assemblyScanner = assemblyScanner;
            this.addressProvider = addressProvider;
            this.conventions = conventions;
        }

        public void Build(IServiceBusHost host, IServiceBusBusFactoryConfigurator configurator)
        {
            addressProvider.Register("error");

            var consumerTypes = GetConsumerTypes();
            var messageTypes = GetMessageTypes();

            foreach (var messageType in messageTypes)
            {
                addressProvider.Register(messageType);
                var messageConsumerTypes = GetConsumerTypes(messageType, consumerTypes);

                if (messageConsumerTypes.Any())
                {
                    foreach (var consumerType in messageConsumerTypes)
                    {
                        RegisterConsumerType(consumerType);

                        var queueName = addressProvider.GetQueueName(messageType);
                        var receiveConfigurator = GetReceiveEndpointConfigurator(messageType, consumerType);
                        
                        configurator.ReceiveEndpoint(host, queueName, receiveConfigurator);
                    }
                }
            }
        }

        private void RegisterConsumerType(Type consumerType)
        {
            container.Configure(x => x.For(consumerType).Use(consumerType));
        }

        private Action<IServiceBusReceiveEndpointConfigurator> GetReceiveEndpointConfigurator(Type messageType, Type consumerType)
        {
            return c =>
            {
                c.EnableDeadLetteringOnMessageExpiration = true;
                c.SupportOrdering = true;
                c.SubscribeMessageTopics = false;
                c.MaxDeliveryCount = 3;
                c.EnableDeadLetteringOnMessageExpiration = true;
                c.UseRetry(Retry.Exponential(3, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(1)));

                if (IsTransactional(consumerType))
                {
                    c.UseTransaction(x =>
                    {
                        x.IsolationLevel = IsolationLevel.ReadCommitted;
                        x.Timeout = TimeSpan.FromMinutes(10);
                    });
                }
                
                c.Consumer(consumerType, t => container.GetInstance(t));
            };
        }

        private bool IsTransactional(Type consumerType)
        {
            return consumerType.GetCustomAttributes(typeof(TransactionalAttribute), true).Length > 0;
        }

        private IEnumerable<Type> GetConsumerTypes(Type messageType, IEnumerable<Type> consumerTypes)
        {
            var messageConsumerTypes = consumerTypes.Where(t => 
                t.GetInterfaces().Any(i => i.IsGenericType && i.UnderlyingSystemType.GetGenericArguments()[0].Equals(messageType))
            );
            return messageConsumerTypes;
        }

        private IEnumerable<Type> GetMessageTypes()
        {
            //messages and commands can be sent ONLY
            var messageTypes = assemblyScanner.Scan(conventions.Messages)
                .Concat(assemblyScanner.Scan(conventions.Commands));

            return messageTypes;
        }

        private IEnumerable<Type> GetEventTypes()
        {
            return assemblyScanner.Scan(conventions.Events);
        }

        private List<Type> GetConsumerTypes()
        {
            var types = assemblyScanner.Scan(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition().Equals(typeof(IConsumer<>))));
            return types.ToList();
        }
    }
}
