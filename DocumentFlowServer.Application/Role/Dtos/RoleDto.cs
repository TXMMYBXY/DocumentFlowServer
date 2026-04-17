using System.Text.Json.Serialization;

namespace DocumentFlowServer.Application.Role.Dtos;

public class RoleDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}