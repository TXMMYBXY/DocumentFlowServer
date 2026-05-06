using System.Security.Claims;
using DocumentFlowServer.Api.Authorization.Requirements;
using DocumentFlowServer.Entities.Enums;
using Microsoft.AspNetCore.Authorization;

namespace DocumentFlowServer.Api.Authorization.Handler;

public class RoleHandler : AuthorizationHandler<RoleIdRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, RoleIdRequirement requirement)
    {
        var isActiveClaim = context.User.FindFirst("IsActive")?.Value;
        var roleId = context.User.FindFirst(ClaimTypes.Role)?.Value;
        
        if(!bool.TryParse(isActiveClaim, out var isActive))
            return Task.CompletedTask;

        if (!Enum.TryParse<Role>(roleId, out var role))
            return Task.CompletedTask;
        
        if (isActive &&
            requirement.AllowedRoles.Contains(role))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}