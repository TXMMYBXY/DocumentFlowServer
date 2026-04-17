using DocumentFlowServer.Application.Issue.Dtos;

namespace DocumentFlowServer.Application.Issue;

public interface IIssueService
{
    Task<IssueResultDto> CreateIssueAsync(CreateIssueRequestDto dto);
}