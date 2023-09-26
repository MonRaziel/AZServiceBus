using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCServiceBusAndLogs.Logic.Models.Configuration
{
    [ExcludeFromCodeCoverage]
    public class ServiceBusConfiguration
    {
        public string TopicName1 { get; set; }
        public string TopicName2 { get; set; }
        public string SubscriptionName { get; set; }
        public string ServiceBusConnectionString { get; set; }

        public static readonly string ConfigurationSectionName = "ServiceBus";
    }
}
