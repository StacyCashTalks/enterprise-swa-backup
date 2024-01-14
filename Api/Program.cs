using Api.Models;
using CosmosDBAccessor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var environmentVariables = Environment.GetEnvironmentVariables();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        {
            services.AddSingleton<IContainerAccess<CosmosTodo>>(factory => new CosmosContainerAccess<CosmosTodo>(
                new CosmosEndpointKeySettings(
                    (string)environmentVariables["CosmosEndpoint"]!,
                    (string)environmentVariables["CosmosKey"]!,
                    (string)environmentVariables["TodoDatabaseName"]!,
                    (string)environmentVariables["TodoContainerName"]!
                )));
            services.AddSingleton<TodoHandler>();
        }
    })
    .Build();

host.Run();
