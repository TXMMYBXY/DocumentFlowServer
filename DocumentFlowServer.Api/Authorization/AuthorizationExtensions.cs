using DocumentFlowServer.Api.Authorization.Handler;
using DocumentFlowServer.Api.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace DocumentFlowServer.Api.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationPolicy(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.Requirements.Add(new RoleIdRequirement(1)));
            options.AddPolicy("AdminAndBoss", policy => policy.Requirements.Add(new RoleIdRequirement(1, 2)));
            options.AddPolicy("AdminBossAndPurchaser", policy => policy.Requirements.Add(new RoleIdRequirement(1, 2, 3)));
            options.AddPolicy("All", policy => policy.Requirements.Add(new RoleIdRequirement(1, 2, 3, 4)));
        });
        
        services.AddSingleton<IAuthorizationHandler, RoleIdHandler>();
        
        return services;
    }
}