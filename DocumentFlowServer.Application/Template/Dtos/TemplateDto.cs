using System.Text.Json.Serialization;
using DocumentFlowServer.Application.User.Dtos;

namespace DocumentFlowServer.Application.Template.Dtos;

public class TemplateDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    public string Path { get; set; }
    
    [JsonPropertyName("createdBy")]
    public TemplateOwnerDto CreatedBy { get; set; }
    
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
    
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
}