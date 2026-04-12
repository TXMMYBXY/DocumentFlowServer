using DocumentFlowServer.Api;
using DocumentFlowServer.Infrastructure;
using DocumentFlowServer.Infrastructure.Data;
using DocumentFlowServer.Infrastructure.Hubs;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApi(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    
    await seeder.SeedAsync();
}

app.MapSwagger("/openapi/{documentName}.json");
app.MapScalarApiReference();
app.MapControllers();
app.MapHub<NotificationHub>("/notifications");

app.Run();
