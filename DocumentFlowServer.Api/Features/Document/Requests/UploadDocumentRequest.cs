using System.ComponentModel.DataAnnotations;
using DocumentFlowServer.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowServer.Api.Features.Document.Requests;

public class UploadDocumentRequest
{
    [Required]
    [FromForm(Name = "Title")]
    public string Title { get; set; }
    
    [Required]
    [FromForm(Name = "CreatedBy")]
    public int? CreatedBy { get; set; }
    
    [Required]
    [FromForm(Name = "TemplateId")]
    public int? TemplateId { get; set; }
    
    [Required]
    [FromForm(Name = "DocumentType")]
    public TemplateType DocumentType { get; set; }
    
    [Required]
    [FromForm(Name = "File")]
    public IFormFile File { get; set; } = null!;
}