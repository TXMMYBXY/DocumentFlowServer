using System.ComponentModel.DataAnnotations;
using DocumentFlowServer.Entities.Enums;
using DocumentFlowServer.Entities.Models;
using TaskStatus = DocumentFlowServer.Entities.Enums.TaskStatus;

namespace DocumentFlowServer.Application.Services.Tasks.Dto;

public class TaskFilterDto
{
    public TaskStatus? Status { get; set; }
    public TemplateType? TemplateType { get; set; }
    public int? UserId { get; set; }
    public TaskPriority? Priority { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public DateTime? UpdatedFrom { get; set; }
    public DateTime? UpdatedTo { get; set; }

    [Range(1, 100)]
    public int Page { get; set; } = 1;
    
    [Range(1, 100)]
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; } = nameof(TaskModel.CreatedAt);
    public bool SortDescending { get; set; } = true;
}
