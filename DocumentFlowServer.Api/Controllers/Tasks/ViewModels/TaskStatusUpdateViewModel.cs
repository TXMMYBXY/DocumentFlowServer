using System.ComponentModel.DataAnnotations;

namespace DocumentFlowServer.Api.Controllers.Tasks.ViewModels;

public class TaskStatusUpdateViewModel
{
    [EnumDataType(typeof(TaskStatus), ErrorMessage = "Неверный статус")]
    public TaskStatus Status { get; set; }

    [StringLength(511)]
    public string? ResultFilePath { get; set; }
    public string? ErrorMessage { get; set; }
}
