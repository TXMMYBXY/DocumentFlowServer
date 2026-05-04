using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DocumentFlowServer.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowServer.Api.Features.Templates.Requests;

public class CreateTemplateRequest
{
    [Required]
    [FromForm(Name="Title")]
    public string Title { get; set; }

    [FromForm(Name="IsActive")]
    public bool IsActive { get; set; }
    
    [Required]
    [FromForm(Name="Type")]
    public TemplateType Type { get; set; }
    
    [Required]
    [FromForm(Name="File")]
    public IFormFile File { get; set; } = null!;
}