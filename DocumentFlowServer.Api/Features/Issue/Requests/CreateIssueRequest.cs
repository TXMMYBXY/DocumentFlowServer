using System.ComponentModel.DataAnnotations;
using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Api.Features.Issue.Requests;

public class CreateIssueRequest
{
    public int TemplateId { get; set; }

    [EnumDataType(typeof(TemplateType))]
    public TemplateType TemplateType { get; set; }
    public Dictionary<string, object> Data { get; set; } = new();

    [EnumDataType(typeof(IssuePriority))]
    public IssuePriority Priority { get; set; } = IssuePriority.Normal;
}