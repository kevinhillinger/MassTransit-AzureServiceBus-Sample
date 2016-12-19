using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nakatomi.Messaging.Configuration
{
    /// <summary>
    /// Defines the message conventions
    /// </summary>
    /// <remarks>
    /// Not required but makes it easy to hook up message contracts to handlers and wireup MassTransit, as well as enforce message contracts at runtime
    /// </remarks>
    public class MessageConventions
    {
        internal Func<Type, bool> Messages { get; set; }
        internal Func<Type, bool> Commands { get; set; }
        internal Func<Type, bool> Events { get; set; }

        internal Func<Assembly, bool> AssembliesToScan { get; set; }

        internal IEnumerable<Func<Type, bool>> AsEnumerable()
        {
            return new[] { Messages, Commands, Events };
        }

        public MessageConventions DefineMessagesAs(Func<Type, bool> definition)
        {
            Messages = definition;
            return this;
        }

        public MessageConventions DefineCommandsAs(Func<Type, bool> definition)
        {
            Commands = definition;
            return this;
        }

        public MessageConventions DefineEventsAs(Func<Type, bool> definition)
        {
            Messages = definition;
            return this;
        }

        public MessageConventions ScanningAssemblies(Func<Assembly, bool> assembliesToScan)
        {
            AssembliesToScan = assembliesToScan;
            return this;
        }
    }
}
