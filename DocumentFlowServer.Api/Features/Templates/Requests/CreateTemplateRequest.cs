using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.Templates.Requests;

public class CreateTemplateRequest
{
    [Required]
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
    
    [Required]
    [JsonPropertyName("file")]
    public IFormFile File { get; set; } = null!;
}