using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.User.Requests;

public class UpdateUserRequest
{
    [JsonPropertyName("fullName")]
    public string? FullName { get; set; }
    
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    
    [JsonPropertyName("departmentId")]
    public int? DepartmentId { get; set; }
    
    [JsonPropertyName("roleId")]
    public int? RoleId { get; set; }
}