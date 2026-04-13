using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.Departments.Requests;

public class CreateDepartmentRequest
{
    [Required]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}