namespace DocumentFlowServer.Application.Services.Template.Dto;

public class TemplateDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Path { get; set; }
    public int CreatedBy { get; set; }
    public virtual Entities.Models.AboutUserModels.User User{ get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}