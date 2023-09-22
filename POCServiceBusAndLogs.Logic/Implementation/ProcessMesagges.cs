using Microsoft.Extensions.Logging;
using POCServiceBusAndLogs.Logic.Interfaces;
using POCServiceBusAndLogs.Logic.Models.Configuration;

namespace POCServiceBusAndLogs.Logic.Implementation
{
    public class ProcessMesagges : IProcessMesagges
    {
        private readonly POCServiceConfiguration _configuration;
        private readonly ILogger<ProcessMesagges> _logger;

        public ProcessMesagges(ILogger<ProcessMesagges> logger, POCServiceConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public void ProcessServiceBusMessage(string message)
        {
            _logger.LogInformation(message);
            //make logic for accesing database, all database extraction could be done using the IoC so in the constructor we can get the EF implementation or things like that.
        }
    }
}
