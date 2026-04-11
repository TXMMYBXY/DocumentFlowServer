using DocumentFlowServer.Application.Repository.Role;
using DocumentFlowServer.Entities.Data;
using DocumentFlowServer.Entities.Models.AboutUserModels;

namespace DocumentFlowServer.Infrastructure.Repository;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
