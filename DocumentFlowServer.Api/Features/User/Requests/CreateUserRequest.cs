using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
    [JsonPropertyName("roleId")]
    public int RoleId { get; set; }
}