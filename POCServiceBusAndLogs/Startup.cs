using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Microsoft.ApplicationInsights.Extensibility;
using POCServiceBusAndLogs.Logic.Models.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ApplicationInsights.AspNetCore;
using POCServiceBusAndLogs.Services;

[assembly: FunctionsStartup(typeof(POCServiceBusAndLogs.Startup))]
namespace POCServiceBusAndLogs
{
    internal class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var azureTenantId = Environment.GetEnvironmentVariable("AZURE_TENANT_ID");
            Environment.SetEnvironmentVariable("AZURE_TENANT_ID", azureTenantId);

            var currentDirectory = "/home/site/wwwroot";
            var isLocal = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"));
            if (isLocal)
            {
                currentDirectory = Environment.CurrentDirectory;
            }
            builder.ConfigurationBuilder.SetBasePath(currentDirectory)
                       .AddJsonFile("local.settings.json", false, true)
                       .AddJsonFile($"{environment}.settings.json", true);
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;
            var logger = ConfigureSerilog(configuration);

            var ServiceConfiguration = new POCServiceConfiguration();
            configuration.GetSection(POCServiceConfiguration.ConfigurationSectionName).Bind(ServiceConfiguration);

            builder.Services.AddSingleton(ServiceConfiguration);
            builder.Services.AddLogging(configure => configure.AddSerilog(logger));
            builder.Services.AddApplicationInsightsTelemetry();
            builder.Services.ConfigureTelemetryModule<RequestTrackingTelemetryModule>((req, o) => req.CollectionOptions.TrackExceptions = false);
            
            //Actual function implementation
            builder.Services.AddManagersDependency();
            
            /*
             * e.g DB Context
             builder.Services.AddDbContext<DbContextClass>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ConnectionStringParamName"))
                );
             */
        }

        private static Logger ConfigureSerilog(IConfiguration configuration)
        {
            var connectionString = Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING");
            var telemetry = TelemetryConfiguration.CreateDefault();
            telemetry.ConnectionString = connectionString;

            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Warning)
                .WriteTo.ApplicationInsights(telemetry, TelemetryConverter.Traces, LogEventLevel.Information)
                .Filter
                .ByExcluding(logEvent => logEvent.Exception != null);

            var overrides = new Dictionary<string, string>();
            configuration.GetSection("Serilog:MinimumLevel:Override").Bind(overrides);
            foreach (var key in overrides.Keys)
            {
                loggerConfiguration.MinimumLevel.Override(key, LogEventLevel.Error);
            }

            return loggerConfiguration.CreateLogger();
        }
    }
}
