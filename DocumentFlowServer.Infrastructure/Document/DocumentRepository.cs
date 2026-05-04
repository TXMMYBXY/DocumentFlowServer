using DocumentFlowServer.Application.Document;
using DocumentFlowServer.Application.Document.Dtos;
using DocumentFlowServer.Application.Role.Dtos;
using DocumentFlowServer.Application.User.Dtos;
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
}