using DocumentFlowServer.Application.Services.Role.Dto;

namespace DocumentFlowServer.Application.Services.Role;

public interface IRoleService
{
    Task<List<GetRoleDto>> GetAllRolesAsync();
}