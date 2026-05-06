using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Api.Features.User.Requests;

public class CreateUserRequest
{
    [Required]
    [JsonPropertyName("fullName")]
    public string FullName { get; set; }
    
    [Required]
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [Required]
    [JsonPropertyName("password")]
    public string Password { get; set; }
    
    [Required]
    [JsonPropertyName("departmentId")]
    public int DepartmentId { get; set; }
    
    [Required]
    [JsonPropertyName("role")]
    public Role Role { get; set; }
}