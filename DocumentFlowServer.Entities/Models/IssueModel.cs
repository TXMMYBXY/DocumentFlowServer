using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Entities.Models;

/// <summary>
/// Model for a document filling task
/// </summary>
public class IssueModel : EntityBase
{
    [Required]
    public Guid TaskId { get; set; } = Guid.NewGuid();
    [Required]
    public int TemplateId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TemplateType TemplateType { get; set; } = TemplateType.Statement;
    [Required]
    public string TemplateData { get; set; } = "{}";
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public IssueStatus Status { get; set; } = IssueStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    [StringLength(511)]
    public string? ResultFilePath { get; set; }
    public string? ErrorMessage { get; set; }
    public int? UserId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public IssuePriority Priority{ get; set; } = IssuePriority.Low;
    // For retries
    public int RetryCount { get; set; } = 0;
    public DateTime? LastAttemptAt { get; set; }
}