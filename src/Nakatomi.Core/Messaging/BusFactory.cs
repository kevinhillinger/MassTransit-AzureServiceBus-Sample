using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Nakatomi.Messaging.Configuration;
using Nakatomi.Messaging.Topology;
using Nakatomi.Utilities;
using StructureMap;
using System;
using System.Reflection;

namespace Nakatomi.Messaging
{
    sealed class BusFactory : IBusFactory
    {
        BusConfiguration config;
        readonly IContainer container;

        public BusFactory(IContainer container)
        {
            this.container = container;
        }

        IBus IBusFactory.Create(BusConfiguration configuration)
        {
            config = configuration;

            container.Configure(x =>
            {
                x.For<IAddressProvider>().Use<AddressProvider>().Singleton().Ctor<Uri>().Is(new Uri(config.AzureServiceBus.Uri));
                x.ForSingletonOf<MessageConventions>().Use(config.Conventions);
                x.For<IAssemblyScanner>().Use<AssemblyScanner>().Ctor<Func<Assembly, bool>>().Is(config.Conventions.AssembliesToScan);
                x.For<RouteBuilder>().Use<RouteBuilder>().Singleton();

                x.For<IBus>().Use<InternalBus>().Singleton().Ctor<string>("name").Is(configuration.Name);
                x.Forward<IBus, ISendMessages>();
            });

            var busControl = GetBusControl(configuration);
            container.Configure(x => x.For<IBusControl>().Use(busControl).Named(configuration.Name));

            return container.With<string>(configuration.Name).GetInstance<IBus>();
        }

        private IBusControl GetBusControl(BusConfiguration configuration)
        {
            var routeBuilder = container.GetInstance<RouteBuilder>();
            var busControl = Bus.Factory.CreateUsingAzureServiceBus(c =>
            {
                var host = c.Host(new Uri(configuration.AzureServiceBus.Uri), ConfigureHost);
                routeBuilder.Build(host, c);
            
            });
            return busControl;
        }

        private void ConfigureHost(IServiceBusHostConfigurator host)
        {
            host.OperationTimeout = TimeSpan.FromSeconds(60);
            host.TokenProvider = GetTokenProvider(config.AzureServiceBus);
            host.TransportType = TransportType.Amqp;
        }

        private TokenProvider GetTokenProvider(AzureServiceBusSettings settings)
        {
            return TokenProvider.CreateSharedAccessSignatureTokenProvider(settings.KeyName, settings.SharedAccessKey, TokenScope.Namespace);
        }
    }
}
