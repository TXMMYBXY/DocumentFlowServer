using DocumentFlowServer.Worker;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

var workerApiKey = Environment.GetEnvironmentVariable("WORKER_API_KEY");
if (!string.IsNullOrWhiteSpace(workerApiKey) && builder.Environment.IsProduction())
{
    builder.Configuration["WorkerSettings:ApiKey"] = workerApiKey;
}

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var host = builder.Build();
host.Run();
