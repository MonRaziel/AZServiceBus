using Microsoft.Extensions.DependencyInjection;
using POCServiceBusAndLogs.Logic.Implementation;
using POCServiceBusAndLogs.Logic.Interfaces;

namespace POCServiceBusAndLogs.Services
{
    internal static class IoC
    {
        public static IServiceCollection AddManagersDependency(this IServiceCollection services)
        {
            //Add here any IoC needed for the project it self, related to the actual work of the function, not the generic functionalities for the AZ to work


            services.AddSingleton<IProcessMesagges, ProcessMesagges>();

            return services;
        }
    }
}
