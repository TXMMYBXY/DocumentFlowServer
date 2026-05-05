using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFlowServer.Application.Document;
using DocumentFlowServer.Application.Document.Dtos;
using DocumentFlowServer.Application.Role.Dtos;
using DocumentFlowServer.Application.Template.Dtos;
using DocumentFlowServer.Application.User.Dtos;
using DocumentFlowServer.Entities.Enums;
using DocumentFlowServer.Infrastructure.Common.Repository;
using DocumentFlowServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Document;

public class DocumentRepository : BaseRepository<Entities.Models.Document>, IDocumentRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public DocumentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DocumentDto> GetFilledDocumentByIdAsync(int documentId)
    {
        return await _dbContext.Documents
            .Where(d => d.Id == documentId)
            .Select(d => new DocumentDto
            {
                Title = d.Title,
                FilePath = d.Path,
                User = new UserCleanDto
                {
                    Id = d.User.Id,
                    Email = d.User.Email,
                    FullName = d.User.FullName,
                    IsActive = d.User.IsActive,
                    Role = new RoleDto
                    {
                        Id = d.User.RoleId,
                        Title = d.User.Role.Title
                    },
                },
                CreatedAt = d.CreatedAt,
                Type = d.Template.Type
            })
            .FirstAsync();
    }

    public async Task<ICollection<DocumentDto>> GetAllDocumentsAsync(int ownerId, DocumentFilter filter)
    {
        var query = _dbContext.Documents
            .AsNoTracking()
            .Where(d => d.CreatedBy == ownerId);
        
        switch (filter.SortByField)
        {
            case "Title":
                if (filter.Descending)
                    query = query.OrderBy(t => t.Title);
                else
                    query = query.OrderByDescending(t => t.Title);
                break;
            
            case "CreatedBy":
                if (filter.Descending)
                    query = query.OrderBy(t => t.CreatedBy);
                else
                    query = query.OrderByDescending(t => t.CreatedBy);
                break;
            
            case "TemplateId":
                if (filter.Descending)
                    query = query.OrderBy(t => t.TemplateId);
                else
                    query = query.OrderByDescending(t => t.TemplateId);
                break;
            
            case "CreatedAt":
                if (filter.Descending)
                    query = query.OrderBy(t => t.CreatedAt);
                else
                    query = query.OrderByDescending(t => t.CreatedAt);
                break;
            
                default:
                    query = query.OrderByDescending(t => t.CreatedAt);
                    break;
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Title))
            query = query.Where(u => u.Title.Contains(filter.Title));

        if (filter.CreatedAtEarlier != null) 
            query = query.Where(t => t.CreatedAt <= filter.CreatedAtEarlier.Value);
        
        if (filter.CreatedAtLater != null) 
            query = query.Where(t => t.CreatedAt >= filter.CreatedAtLater.Value);

        if (filter.TemplateId.HasValue)
            query = query.Where(u => u.TemplateId == filter.TemplateId.Value);

        var resultQuery = query
            .Select(d => new DocumentDto
            {
                Id = d.Id,
                Title = d.Title,
                FilePath = d.Path,
                CreatedAt = d.CreatedAt,
                Type = d.Type,
                Template = new TemplateClearDto
                {
                    Id = d.TemplateId,
                    Title = d.Template.Title
                },
                User = new UserCleanDto
                {
                    Id = d.User.Id,
                    Email = d.User.Email,
                    FullName = d.User.FullName,
                    IsActive = d.User.IsActive,
                    Role = new RoleDto
                    {
                        Id = d.User.RoleId,
                        Title = d.User.Role.Title
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
}