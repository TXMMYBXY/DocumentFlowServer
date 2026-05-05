using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFlowServer.Application.Role.Dtos;
using DocumentFlowServer.Application.Template;
using DocumentFlowServer.Application.Template.Dtos;
using DocumentFlowServer.Application.User.Dtos;
using DocumentFlowServer.Entities.Enums;
using DocumentFlowServer.Infrastructure.Common.Repository;
using DocumentFlowServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Template;

public class TemplateRepository : BaseRepository<Entities.Models.Template>, ITemplateRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TemplateRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<(ICollection<TemplateDto>, int)> GetAllTemplatesAsync(TemplateFilter filter)
    {
        var query = _dbContext.Templates
            .Where(t => t.Type == filter.Type)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Title))
            query = query.Where(u => u.Title.Contains(filter.Title.ToLower()));

        if (filter.CreatedBy.HasValue) 
            query = query.Where(t => t.CreatedBy == filter.CreatedBy.Value);
        
        if (filter.CreatedAtEarlier != null) 
            query = query.Where(t => t.CreatedAt <= filter.CreatedAtEarlier.Value);
        
        if (filter.CreatedAtLater != null) 
            query = query.Where(t => t.CreatedAt >= filter.CreatedAtLater.Value);
        
        var totalCount = await query.CountAsync();
        
        var resultQuery = query
            .OrderByDescending(t => t.CreatedAt)
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

        return (await resultQuery.ToListAsync(), totalCount);
    }

    public async Task<TemplateDto?> GetTemplateForDownloadingByIdAsync(int templateId)
    {
        return await _dbContext.Templates
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
        await _dbContext.Templates
            .Where(t => t.Id == templateId)
            .ExecuteUpdateAsync(
                setter => setter.SetProperty(x => x.IsActive, x => !x.IsActive)
            );

        return await _dbContext.Templates
            .Where(t => t.Id == templateId)
            .Select(t => t.IsActive)
            .SingleOrDefaultAsync();
    }

    public async Task<string?> GetFilePathAsync(int templateId)
    {
        return await _dbContext.Templates
            .Where(t => t.Id == templateId)
            .Select(t => t.Path)
            .SingleOrDefaultAsync();
    }

    public async Task UpdateTemplatePartialAsync(int templateId, string? title, string? path)
    {
        var query = _dbContext.Templates
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

        await query.ExecuteUpdateAsync(s => s
            .SetProperty(t => t.CreatedAt, DateTime.UtcNow));
    }

    public async Task<TemplateType> GetTypeByTemplateIdAsync(int templateId)
    {
        return await _dbContext.Templates
            .Where(t => t.Id == templateId)
            .Select(t => t.Type)
            .SingleAsync();
    }

    public async Task<List<GetTemplateDto>> GetTemplatesWithoutContractsAsync()
    {
        return await _dbContext.Templates
            .Where(t => t.Type != TemplateType.Contract)
            .Select(t => new GetTemplateDto
            {
                Id = t.Id,
                Title = t.Title
            })
            .ToListAsync();
    }

    public async Task<List<GetTemplateDto>> GetTemplatesAsync()
    {
        return await _dbContext.Templates
            .Select(t => new GetTemplateDto
            {
                Id = t.Id,
                Title = t.Title
            })
            .ToListAsync();
    }
}