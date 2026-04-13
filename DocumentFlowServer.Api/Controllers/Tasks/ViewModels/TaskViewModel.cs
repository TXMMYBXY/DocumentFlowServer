using DocumentFlowServer.Entities.Enums;
using TaskStatus = DocumentFlowServer.Entities.Enums.TaskStatus;

namespace DocumentFlowServer.Api.Controllers.Tasks.ViewModels;

public class TaskViewModel
{
    public Guid TaskId { get; set; }
    public int TemplateId { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ResultFilePath { get; set; }
    public string? ErrorMessage { get; set; }
    public int? UserId { get; set; }
    public int RetryCount { get; set; }
    public bool CanRetry { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDownload => Status == TaskStatus.Completed && !string.IsNullOrEmpty(ResultFilePath);
    public string? DownloadUrl { get; set; }

}
