using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Application.Issue.Dtos;

public class CreateIssueRequestDto
{
    public int TemplateId { get; set; }
    public TemplateType TemplateType { get; set; }
    public Dictionary<string, object> Data { get; set; } = new();
    public int? UserId { get; set; }
    public IssuePriority Priority { get; set; } = IssuePriority.Normal;
}