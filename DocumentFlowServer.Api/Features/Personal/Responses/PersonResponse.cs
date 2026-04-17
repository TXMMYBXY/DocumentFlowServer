using System.Text.Json.Serialization;
using DocumentFlowServer.Application.Role.Dtos;

namespace DocumentFlowServer.Api.Features.Personal.Responses;

public class PersonResponse
{
    [JsonPropertyName("fullName")]
    public string FullName { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("department")]
    public string Department { get; set; }
    
    [JsonPropertyName("role")]
    public RoleDto Role { get; set; }
}