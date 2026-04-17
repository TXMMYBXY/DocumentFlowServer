using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Application.Issue.Dtos;

public class IssueResultDto
{
    public Guid TaskId { get; set; }
    public IssueStatus Status { get; set; }
    public string? ResultFilePath { get; set; }
    public string? DownloadUrl { get; set; }
    public string? ErrorMessage { get; set; }
    public string Message { get; set; } = string.Empty;
}