using System.ComponentModel.DataAnnotations.Schema;
using DocumentFlowServer.Application.Services.User.Dto;

namespace DocumentFlowServer.Application.Services.Template.Dto;

public class GetTemplateDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Path { get; set; }
    public int CreatedBy { get; set; }
    [ForeignKey(nameof(CreatedBy))]
    public virtual GetUserDto User { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; }
}