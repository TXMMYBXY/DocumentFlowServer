using System.ComponentModel.DataAnnotations;
using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Api.Features.Templates.Requests;

public class GetTemplatesRequest
{
    [Required]
    public TemplateType Type { get; set; }
    
    public string? Title { get; set; }
    public int? CreatedBy { get; set; } 
    public DateTime? CreatedAtEarlier { get; set; }
    public DateTime? CreatedAtLater { get; set; }
    
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
}