using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DocumentFlowServer.Entities.Enums;
using DocumentFlowServer.Entities.Models.AboutUserModels;

namespace DocumentFlowServer.Entities.Models;

/// <summary>
/// Model with metadat of filed document
/// </summary>
public class Document : EntityBase
{
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Path { get; set; }
    
    [Required]
    public int CreatedBy { get; set; }
    
    [ForeignKey(nameof(CreatedBy))]
    public virtual User User { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public int TemplateId { get; set; }
    
    [ForeignKey(nameof(TemplateId))]
    public virtual Template Template { get; set; }
}