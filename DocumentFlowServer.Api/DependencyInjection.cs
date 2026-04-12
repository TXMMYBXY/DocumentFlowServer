using Microsoft.OpenApi.Models;

namespace DocumentFlowServer.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        
        services.AddAutoMapper(typeof(Program));
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddSignalR();
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Document Flow API",
                Version = "v1"
            });

            // Добавляем поддержку JWT авторизации в Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetSection("Redis:Configuration").Value;
            options.InstanceName = configuration.GetSection("Redis:InstanceName").Value;
        });

        return services;
    }
}