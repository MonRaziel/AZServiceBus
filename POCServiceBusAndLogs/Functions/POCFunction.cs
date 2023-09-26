using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using POCServiceBusAndLogs.Logic.Interfaces;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs.ServiceBus;

namespace POCServiceBusAndLogs
{
    public class POCFunction
    {
        private readonly ILogger<POCFunction> _logger;
        private readonly IProcessMesagges _processMesagges;

        public POCFunction(ILogger<POCFunction> log, IProcessMesagges messageManager)
        {
            _logger = log;
            _processMesagges = messageManager;
        }

        [FunctionName("POCFunction")]
        public async Task Process([ServiceBusTrigger("%ServiceBus:TopicName1%", "%ServiceBus:SubscriptionName%", Connection = "ConnectionStrings:ServiceBusConnectionString", AutoCompleteMessages = false)]
            ServiceBusReceivedMessage serviceBusReceivedMessage,
            ServiceBusMessageActions messageActions
            )
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {serviceBusReceivedMessage.Body}");

            var message = Encoding.UTF8.GetString(serviceBusReceivedMessage.Body);
            //var realtimeMessage = JsonConvert.DeserializeObject<TopicMessage>(message);

            _processMesagges.ProcessServiceBusMessage(message);
            //if result is ok dequeu the message
            await messageActions.CompleteMessageAsync(serviceBusReceivedMessage);

            //depend on what happen in the process could be different actions
            /*
             * await messageActions.AbandonMessageAsync(serviceBusReceivedMessage);
             * await messageActions.DeadLetterMessageAsync(serviceBusReceivedMessage);
             */


            //chatch issues

        }
    }
}
