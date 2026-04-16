using DocumentFlowServer.Api.Authorization.Requirements;
using DocumentFlowServer.Entities.Enums;
using Microsoft.AspNetCore.Authorization;

namespace DocumentFlowServer.Api.Authorization.Handler;

public class RoleIdHandler : AuthorizationHandler<RoleIdRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, RoleIdRequirement requirement)
    {
        var isActiveClaim = context.User.FindFirst("IsActive")?.Value;
        var roleId = context.User.FindFirst("RoleId")?.Value;
        
        if(!bool.TryParse(isActiveClaim, out var isActive))
            return Task.CompletedTask;

        if (!Enum.TryParse<Permissions>(roleId, out var role))
            return Task.CompletedTask;
        
        if (isActive &&
            requirement.AllowedRoles.Contains(role))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}