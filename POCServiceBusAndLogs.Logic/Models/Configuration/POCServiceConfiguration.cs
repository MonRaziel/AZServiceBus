using System.Diagnostics.CodeAnalysis;

namespace POCServiceBusAndLogs.Logic.Models.Configuration
{
    [ExcludeFromCodeCoverage]
    public class POCServiceConfiguration
    {
        public string MyProperty1 { get; set; }
        public string MyProperty2 { get; set; }
        public string ServiceBusConnectionString { get; set; }

        public static readonly string ConfigurationSectionName = "POCServiceConfiguration";
    }
}
