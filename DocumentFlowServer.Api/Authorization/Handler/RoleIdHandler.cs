using DocumentFlowServer.Api.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace DocumentFlowServer.Api.Authorization.Handler;

public class RoleIdHandler : AuthorizationHandler<RoleIdRequirement>
{
    protected override async Task<Task> HandleRequirementAsync(
        AuthorizationHandlerContext context, RoleIdRequirement requirement)
    {
        var isActiveClaim = context.User.FindFirst("IsActive")?.Value;
        var roleId = context.User.FindFirst("RoleId")?.Value;
        
        if(!bool.TryParse(isActiveClaim, out var isActive))
        {
            return Task.CompletedTask;
        }

        if (isActive &&
            requirement.AllowedRoles.Contains(int.Parse(roleId)))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}