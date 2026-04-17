using System.Text.Json.Serialization;
using DocumentFlowServer.Application.Department.Dtos;
using DocumentFlowServer.Application.Role.Dtos;

namespace DocumentFlowServer.Application.User.Dtos;

public class UserDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("fullName")]
    public string FullName { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
    
    [JsonPropertyName("department")]
    public DepartmentCleanDto Department { get; set; }
    
    [JsonPropertyName("role")]
    public RoleDto Role { get; set; }
}