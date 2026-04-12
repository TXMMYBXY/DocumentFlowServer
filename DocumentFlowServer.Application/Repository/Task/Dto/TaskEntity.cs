using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DocumentFlowServer.Entities.Enums;
using DocumentFlowServer.Entities.Models;
using TaskStatus = DocumentFlowServer.Entities.Enums.TaskStatus;

namespace DocumentFlowServer.Application.Repository.Task.Dto;

public class TaskEntity
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
    public Entities.Enums.TaskStatus Status { get; set; } = TaskStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    [StringLength(511)]
    public string? ResultFilePath { get; set; }
    public string? ErrorMessage { get; set; }
    public int? UserId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TaskPriority Priority{ get; set; } = TaskPriority.Low;
    // Для повторных попыток
    public int RetryCount { get; set; } = 0;
    public DateTime? LastAttemptAt { get; set; }

    public TaskEntity(TaskModel model)
    {
        TaskId = model.TaskId;
        TemplateId = model.TemplateId;
        TemplateType = model.TemplateType;
        TemplateData = model.TemplateData;
        Status = model.Status;
        CreatedAt = model.CreatedAt;
        UpdatedAt = model.UpdatedAt;
        StartedAt = model.StartedAt;
        CompletedAt = model.CompletedAt;
        ResultFilePath = model.ResultFilePath;
        ErrorMessage = model.ErrorMessage;
        UserId = model.UserId;
        Priority = model.Priority;
        RetryCount = model.RetryCount;
        LastAttemptAt = model.LastAttemptAt;
    }
}