using DocumentFlowServer.Application.Common.MappingProfiles;
using DocumentFlowServer.Application.Common.Reposiroty;
using DocumentFlowServer.Application.Common.Services;
using DocumentFlowServer.Application.User;
using DocumentFlowServer.Infrastructure.Common.Repository;
using DocumentFlowServer.Infrastructure.Common.Services;
using DocumentFlowServer.Infrastructure.Data;
using DocumentFlowServer.Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentFlowServer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
        
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<DataSeeder>();
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseLazyLoadingProxies()
                .UseMySql(
                    configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 21)),
                    options => options.EnableRetryOnFailure());
        });
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetSection("Redis:Configuration").Value;
            options.InstanceName = configuration.GetSection("Redis:InstanceName").Value;
        });
        
        return services;
    }
}