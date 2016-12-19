namespace Nakatomi.Messaging.Configuration
{
    /// <summary>
    /// Settings to hook into the azure service bus transport
    /// </summary>
    public class AzureServiceBusSettings
    {
        public string Uri { get; set; }
        public string KeyName { get; set; }
        public string SharedAccessKey { get; set; }


        public static class SettingName
        {
            public const string Uri = "sb:uri";
            public const string KeyName = "sb:sas:keyName";
            public const string SharedAccessKey = "sb:sas:sharedAccessKey";
        }
    }
}
