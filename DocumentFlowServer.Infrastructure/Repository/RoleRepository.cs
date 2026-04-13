using DocumentFlowServer.Application.Repository.Role;
using DocumentFlowServer.Application.Repository.Role.Dto;
using DocumentFlowServer.Entities.Models.AboutUserModels;
using DocumentFlowServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Repository;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<RoleEntity>> GetRolesAsync()
    {
        return await _dbContext.Roles.Select(r => new RoleEntity
        {
            Id = r.Id,
            Title = r.Title
        }).ToListAsync();
    }
}
