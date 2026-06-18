using DocumentFlowServer.Api;
using DocumentFlowServer.Api.Middleware;
using DocumentFlowServer.Infrastructure;
using DocumentFlowServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using DocumentFlowServer.Infrastructure.Document;
using DocumentFlowServer.Infrastructure.Notification;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var envConn = builder.Configuration["DEFAULT_CONNECTION"] ?? Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");

if (string.IsNullOrWhiteSpace(envConn))
{
    var host = Environment.GetEnvironmentVariable("MYSQL_HOST") ?? Environment.GetEnvironmentVariable("MYSQL_SERVER") ?? "localhost";
    var port = Environment.GetEnvironmentVariable("MYSQL_PORT") ?? "3307";
    var db = Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? "DocumentFlowDB";
    var user = Environment.GetEnvironmentVariable("MYSQL_USER") ?? "root";
    var password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "RootSecure123!";

    envConn = $"Server={host};Port={port};Database={db};User={user};Password={password};SslMode=None;AllowPublicKeyRetrieval=True;";
}


builder.Configuration["ConnectionStrings:DefaultConnection"] = envConn;

builder.Services.AddApi(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Applying EF migrations...");

        await context.Database.MigrateAsync();

        logger.LogInformation("Migrations applied successfully.");
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Failed to apply migrations. Application cannot start.");
        throw;
    }

    try
    {
        var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        await seeder.SeedAsync();
        logger.LogInformation("Seeding completed.");
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Failed to seed database. Application cannot start.");
        throw;
    }
}

app.UseHttpsRedirection();
app.UseErrorHandling();
app.UseAuthentication();
app.UseAuthorization();

app.MapSwagger("/openapi/{documentName}.json");
app.MapScalarApiReference();
app.MapControllers();
app.MapHub<NotificationHub>("/notifications");
app.MapHub<DocumentHub>("/documents");

app.MapGet("/ping", () => "pong");

app.Run();
