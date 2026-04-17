using DocumentFlowServer.Application.Role;
using DocumentFlowServer.Application.Role.Dtos;
using DocumentFlowServer.Infrastructure.Common.Repository;
using DocumentFlowServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Role;

public class RoleRepository : BaseRepository<Entities.Models.AboutUserModels.Role>, IRoleRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ICollection<RoleDto>> GetAllRolesAsync()
    {
        return await _dbContext.Roles
            .Select(r => new RoleDto
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description
            })
            .ToListAsync();
    }
}