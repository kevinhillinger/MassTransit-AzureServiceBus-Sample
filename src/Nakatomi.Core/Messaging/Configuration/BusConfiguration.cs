
using StructureMap;
using System;

namespace Nakatomi.Messaging.Configuration
{
    /// <summary>
    /// Configuration for the message endpoint
    /// </summary>
    public class BusConfiguration
    {
        IContainer container;

        /// <summary>
        /// Gets the azure service bus settings
        /// </summary>
        public AzureServiceBusSettings AzureServiceBus { get; set; }

        /// <summary>
        /// Gets the message conventions (for messages, commands, and events)
        /// </summary>
        public MessageConventions Conventions { get; set; }

        internal string Name { get; }

        public BusConfiguration(string name)
        {
            Name = name;
            AzureServiceBus = new AzureServiceBusSettings();
            Conventions = new MessageConventions();
        }

        /// <summary>
        /// Set the container to use for the bus to be configured with
        /// </summary>
        /// <param name="container"></param>
        public void UseContainer(IContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Gets a new container to use for setup and sets one for the bus configuration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IContainer UseContainer(Action<ConfigurationExpression> c = null)
        {
            return (container = (c == null ? new Container() : new Container(c)));
        }

        internal IContainer GetContainer()
        {
            if (container == null)
            {
                return UseContainer();
            }
            return container;
        }
    }
}
