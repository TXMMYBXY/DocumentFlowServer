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
                new RoleIdRequirement(Permissions.Admin)));
            
            options.AddPolicy(Policy.AdminAndBoss, policy => policy.Requirements.Add(
                new RoleIdRequirement(Permissions.Admin, Permissions.Boss)));
            
            options.AddPolicy(Policy.AdminBossAndPurchasher, policy => policy.Requirements.Add(
                new RoleIdRequirement(Permissions.Admin, Permissions.Boss, Permissions.Purchaser)));
            
            options.AddPolicy(Policy.All, policy => policy.Requirements.Add(
                new RoleIdRequirement(Permissions.Admin, Permissions.Boss, Permissions.Purchaser, Permissions.Staff)));
        });
        
        services.AddSingleton<IAuthorizationHandler, RoleIdHandler>();
        
        return services;
    }
}