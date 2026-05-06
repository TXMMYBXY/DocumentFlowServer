using System.Text.Json.Serialization;
using DocumentFlowServer.Entities.Enums;

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
    public Role Role { get; set; }
}