using DocumentFlowServer.Api;
using DocumentFlowServer.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApi(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapSwagger("/openapi/{documentName}.json");
app.MapScalarApiReference();
app.MapControllers();

app.Run();
