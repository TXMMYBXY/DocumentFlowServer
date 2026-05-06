using DocumentFlowServer.Api.Authorization.Handler;
using DocumentFlowServer.Api.Authorization.Policies;
using DocumentFlowServer.Api.Authorization.Requirements;
using DocumentFlowServer.Entities.Enums;
using Microsoft.AspNetCore.Authorization;

namespace DocumentFlowServer.Api.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationPolicy(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policy.AdminOnly, policy => policy.Requirements.Add(
                new RoleIdRequirement(Role.Administrator)));
            
            options.AddPolicy(Policy.AdminAndBoss, policy => policy.Requirements.Add(
                new RoleIdRequirement(Role.Administrator, Role.Boss)));
            
            options.AddPolicy(Policy.AdminBossAndPurchaser, policy => policy.Requirements.Add(
                new RoleIdRequirement(Role.Administrator, Role.Boss, Role.Purchaser)));
            
            options.AddPolicy(Policy.All, policy => policy.Requirements.Add(
                new RoleIdRequirement(Role.Administrator, Role.Boss, Role.Purchaser, Role.Employee)));
        });
        
        services.AddSingleton<IAuthorizationHandler, RoleHandler>();
        
        return services;
    }
}