using System.ComponentModel.DataAnnotations.Schema;
using DocumentFlowServer.Entities.Enums;
using DocumentFlowServer.Entities.Models.AboutUserModels;

namespace DocumentFlowServer.Entities.Models;

/// <summary>
/// Базовая модель шаблона для документа
/// </summary>
public class Template : EntityBase
{
    public string Title { get; set; }
    public string Path { get; set; }
    public int CreatedBy { get; set; }
    [ForeignKey(nameof(CreatedBy))]
    public virtual User User { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; }
    public TemplateType Type { get; set; }
}