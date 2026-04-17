using System.Text.Json.Serialization;

namespace DocumentFlowServer.Application.Department.Dtos;

/// <summary>
/// Dto без сотрудников
/// </summary>
public class DepartmentCleanDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}