using Microsoft.AspNetCore.Authorization;

namespace DocumentFlowServer.Api.Authorization.Requirements;

public class RoleIdRequirement : IAuthorizationRequirement
{
    public int[] AllowedRoles;
    
    public RoleIdRequirement(params int[] roleIds)
    {
        AllowedRoles = roleIds;
    }
}