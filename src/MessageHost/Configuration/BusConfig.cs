using Nakatomi.Messaging.Configuration;
using StructureMap;
using System.Configuration;

namespace MessageHost.Configuration
{
    class BusConfig
    {
        public static BusConfiguration GetConfig(IContainer container)
        {
            var config = new BusConfiguration("Nakatomi.MessageHost")
            {
                AzureServiceBus = new AzureServiceBusSettings
                {
                    Uri = ConfigurationManager.AppSettings[AzureServiceBusSettings.SettingName.Uri],
                    KeyName = ConfigurationManager.AppSettings[AzureServiceBusSettings.SettingName.KeyName],
                    SharedAccessKey = ConfigurationManager.AppSettings[AzureServiceBusSettings.SettingName.SharedAccessKey]
                },
                Conventions = GetConventions()
            };

            config.UseContainer(container);

            return config;
        }

        private static MessageConventions GetConventions()
        {
            return new MessageConventions()
                .DefineMessagesAs(c => c.Namespace != null && c.Namespace.Contains("Contracts.Messages"))
                .DefineCommandsAs(c => c.Namespace != null && c.Namespace.Contains("Contracts.Commands"))
                .DefineEventsAs(c => c.Namespace != null && c.Namespace.Contains("Contracts.Events"))
                .ScanningAssemblies(a => a.GetName().FullName.StartsWith("Nakatomi"));
        }
    }
}
