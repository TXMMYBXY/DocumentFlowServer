using DocumentFlowServer.Entities.Enums;
using Microsoft.AspNetCore.Authorization;

namespace DocumentFlowServer.Api.Authorization.Requirements;

public class RoleIdRequirement : IAuthorizationRequirement
{
    public Role[] AllowedRoles;
    
    public RoleIdRequirement(params Role[] roles)
    {
        AllowedRoles = roles;
    }
}