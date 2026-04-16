using DocumentFlowServer.Entities.Enums;
using Microsoft.AspNetCore.Authorization;

namespace DocumentFlowServer.Api.Authorization.Requirements;

public class RoleIdRequirement : IAuthorizationRequirement
{
    public Permissions[] AllowedRoles;
    
    public RoleIdRequirement(params Permissions[] roles)
    {
        AllowedRoles = roles;
    }
}