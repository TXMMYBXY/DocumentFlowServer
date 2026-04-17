using System.Text.Json.Serialization;

namespace DocumentFlowServer.Application.Jwt.Dtos;

public class AccessTokenDto
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("expiresAt")]
    public DateTimeOffset ExpiresAt { get; set; }
    
    [JsonPropertyName("tokenType")]
    public string TokenType { get; set; } = "Bearer";
}