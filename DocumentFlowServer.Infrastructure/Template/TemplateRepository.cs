using DocumentFlowServer.Application.Role.Dtos;
using DocumentFlowServer.Application.Template;
using DocumentFlowServer.Application.Template.Dtos;
using DocumentFlowServer.Application.User.Dtos;
using DocumentFlowServer.Infrastructure.Common.Repository;
using DocumentFlowServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Template;

public class TemplateRepository<T> : BaseRepository<Entities.Models.DocumentTemplatesModels.Template>, ITemplateRepository<T> where T : Entities.Models.DocumentTemplatesModels.Template
{
    private readonly ApplicationDbContext _dbContext;

    public TemplateRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ICollection<TemplateDto>> GetAllTemplatesAsync(TemplateFilter filter)
    {
        var query = _dbContext.Set<T>()
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Title))
            query = query.Where(u => u.Title.Contains(filter.Title));

        if (filter.CreatedBy.HasValue) 
            query = query.Where(t => t.CreatedBy == filter.CreatedBy.Value);
        
        if (filter.CreatedAtEarlier != null) 
            query = query.Where(t => t.CreatedAt <= filter.CreatedAtEarlier.Value);
        
        if (filter.CreatedAtLater != null) 
            query = query.Where(t => t.CreatedAt >= filter.CreatedAtLater.Value);

        var resultQuery = query
            .Select(t => new TemplateDto
            {
                Id = t.Id,
                Title = t.Title,
                Path = t.Path,
                CreatedAt = t.CreatedAt,
                IsActive = t.IsActive,
                CreatedBy = new TemplateOwnerDto
                {
                    Id = t.User.Id,
                    Email = t.User.Email,
                    FullName = t.User.FullName,
                    Role = new RoleDto
                    {
                        Id = t.User.Role.Id,
                        Title = t.User.Role.Title,
                    }
                }
            });

        if (filter.PageSize.HasValue && filter.PageNumber.HasValue)
        {
            resultQuery = resultQuery
                .Skip((filter.PageNumber.Value - 1) * filter.PageSize.Value)
                .Take(filter.PageSize.Value);
        }

        return await resultQuery.ToListAsync();
    }

    public async Task<TemplateDto?> GetTemplateByIdAsync(int templateId)
    {
        return await _dbContext.Set<T>()
            .Where(t => t.Id == templateId)
            .Select(t => new TemplateDto
            {
                Title = t.Title,
                Path = t.Path
            })
            .SingleOrDefaultAsync();
    }

    public async Task<bool> UpdateTemplateStatusAsync(int templateId)
    {
        await _dbContext.Set<T>()
            .Where(t => t.Id == templateId)
            .ExecuteUpdateAsync(
                setter => setter.SetProperty(x => x.IsActive, x => !x.IsActive)
            );

        return await _dbContext.Set<T>()
            .Where(t => t.Id == templateId)
            .Select(t => t.IsActive)
            .SingleOrDefaultAsync();
    }

    public async Task<string?> GetFilePathAsync(int templateId)
    {
        return await _dbContext.Set<T>()
            .Where(t => t.Id == templateId)
            .Select(t => t.Path)
            .SingleOrDefaultAsync();
    }

    public async Task UpdateTemplatePartialAsync(int templateId, string? title, string? path)
    {
        var query = _dbContext.Set<T>()
            .Where(t => t.Id == templateId);

        if (title != null && path != null)
        {
            await query.ExecuteUpdateAsync(s => s
                .SetProperty(t => t.Title, title)
                .SetProperty(t => t.Path, path));
        }
        else if (title != null)
        {
            await query.ExecuteUpdateAsync(s => s
                .SetProperty(t => t.Title, title));
        }
        else if (path != null)
        {
            await query.ExecuteUpdateAsync(s => s
                .SetProperty(t => t.Path, path));
        }
    }
}