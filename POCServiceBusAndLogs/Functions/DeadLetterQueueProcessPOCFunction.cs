using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using POCServiceBusAndLogs.Logic.Models.Configuration;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCServiceBusAndLogs.Functions
{
    public  class DeadLetterQueueProcessPOCFunction
    {
        private readonly ILogger<DeadLetterQueueProcessPOCFunction> _logger;
        private readonly string _topicName;

        public DeadLetterQueueProcessPOCFunction(ServiceBusConfiguration serviceBusConfiguration,
            ILogger<DeadLetterQueueProcessPOCFunction> logger)
        {
            _logger = logger;
        }

        [FunctionName("DeadLetterQueueProcessPOCFunction")]
        public async Task Run([TimerTrigger("0 0 4,16 * * *", RunOnStartup = true)] TimerInfo timerInfo)
        {
            
                _logger.LogInformation($"DLQ Process Function starting");
                //call generic manager of the actual function so we do not need to replicate code
                _logger.LogInformation($"DLQ Process Function completed successfully");
        }
    }
}
