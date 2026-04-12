using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Controllers.Department.ViewModels;

public class CreateDepartmentViewModel
{
    [Required]
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}