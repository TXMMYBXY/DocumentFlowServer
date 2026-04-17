using DocumentFlowServer.Application.Issue;
using DocumentFlowServer.Entities.Models;
using DocumentFlowServer.Infrastructure.Common.Repository;
using DocumentFlowServer.Infrastructure.Data;

namespace DocumentFlowServer.Infrastructure.Issue;

public class IssueRepository : BaseRepository<IssueModel>, IIssueRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public IssueRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}