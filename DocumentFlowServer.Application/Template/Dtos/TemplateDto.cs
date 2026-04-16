using DocumentFlowServer.Application.User.Dtos;

namespace DocumentFlowServer.Application.Template.Dtos;

public class TemplateDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Path { get; set; }
    public TemplateOwnerDto CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}