using System.Text.Json.Serialization;
using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Application.User.Dtos;

/// <summary>
/// Dto для возврата информации при авторизации
/// </summary>
public class UserInfoForLoginDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("department")]
    public string Department { get; set; }
    
    [JsonPropertyName("role")]
    public Role Role { get; set; }
}