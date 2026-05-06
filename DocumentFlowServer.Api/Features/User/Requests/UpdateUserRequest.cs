using System.Text.Json.Serialization;
using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Api.Features.User.Requests;

public class UpdateUserRequest
{
    [JsonPropertyName("fullName")]
    public string? FullName { get; set; }
    
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    
    [JsonPropertyName("departmentId")]
    public int? DepartmentId { get; set; }
    
    [JsonPropertyName("role")]
    public Role Role { get; set; }
}