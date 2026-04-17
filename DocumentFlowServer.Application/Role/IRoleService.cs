using DocumentFlowServer.Application.Role.Dtos;

namespace DocumentFlowServer.Application.Role;

public interface IRoleService
{
    Task<ICollection<RoleDto>> GetRolesAsync();
}