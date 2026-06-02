using System;
using DocumentFlowServer.Worker.Client;
using DocumentFlowServer.Worker.Client.Api;
using DocumentFlowServer.Worker.Configuration;
using DocumentFlowServer.Worker.Interface;
using DocumentFlowServer.Worker.Interface.Client;
using DocumentFlowServer.Worker.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DocumentFlowServer.Worker;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<WorkerSettings>(
            Configuration.GetSection("WorkerSettings"));

        services.AddHttpClient<IGeneralClient, GeneralClient>();


        services.AddHttpClient<IApiClient, ApiClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<WorkerSettings>>().Value;

            client.BaseAddress = new Uri(settings.ApiBaseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("X-Worker-Api-Key", settings.ApiKey);
        });

        services.AddSingleton<IDocumentTemplateService, DocumentTemplateService>();

        services.AddHostedService<WorkerService>();
    }
}
