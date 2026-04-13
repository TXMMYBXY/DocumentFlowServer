using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Application.Services.WorkerTask.Dto;

public class WorkerTaskDto
{
    public Guid TaskId { get; set; }
    public int TemplateId { get; set; }
    public TemplateType TemplateType { get; set; }
    public Dictionary<string, object> Data { get; set; }
}
