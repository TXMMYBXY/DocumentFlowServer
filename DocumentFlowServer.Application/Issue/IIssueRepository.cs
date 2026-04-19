using DocumentFlowServer.Application.Common.Reposiroty;
using DocumentFlowServer.Entities.Models;

namespace DocumentFlowServer.Application.Issue;

public interface IIssueRepository : IBaseRepository<IssueModel>
{
    Task<IssueModel?> GetIssueByIdAsync(Guid taskId);
    Task<IssueModel?> GetIssueByStatusPendingAsync();
}