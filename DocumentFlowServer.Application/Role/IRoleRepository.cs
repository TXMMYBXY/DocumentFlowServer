using DocumentFlowServer.Application.Common.Reposiroty;
using DocumentFlowServer.Application.Role.Dtos;

namespace DocumentFlowServer.Application.Role;

public interface IRoleRepository : IBaseRepository<Entities.Models.AboutUserModels.Role>
{
    Task<ICollection<RoleDto>> GetAllRolesAsync();
}