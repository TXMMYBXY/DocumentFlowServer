using System;
using System.Linq;
using System.Threading.Tasks;
using DocumentFlowServer.Application.Issue;
using DocumentFlowServer.Entities.Models;
using DocumentFlowServer.Infrastructure.Common.Repository;
using DocumentFlowServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Issue;

public class IssueRepository : BaseRepository<IssueModel>, IIssueRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public IssueRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IssueModel?> GetIssueByIdAsync(Guid taskId)
    {
        return await _dbContext.Tasks
            .Where(t => t.TaskId == taskId)
            .FirstOrDefaultAsync();
    }

    public async Task<IssueModel?> GetIssueByStatusPendingAsync()
    {
        throw new NotImplementedException();
    }
}