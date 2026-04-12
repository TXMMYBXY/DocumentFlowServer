using DocumentFlowServer.Application.Repository.Role.Dto;

namespace DocumentFlowServer.Application.Repository.Role;

public interface IRoleRepository : IBaseRepository<Entities.Models.AboutUserModels.Role>
{
    Task<List<RoleEntity>> GetRolesAsync();
}