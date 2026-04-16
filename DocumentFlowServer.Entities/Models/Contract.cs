using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DocumentFlowServer.Entities.Enums;
using DocumentFlowServer.Entities.Models.DocumentTemplatesModels;

namespace DocumentFlowServer.Entities.Models;

/// <summary>
/// Model of a non-standard contract (created)
/// </summary>
public class Contract : EntityBase
{
    [Required]
    public string Title { get; set; }
    public string Path { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
    public int TemplateId { get; set; }
    
    [ForeignKey(nameof(ContractTemplate.Id))]
    public virtual ContractTemplate Template { get; set; }
}