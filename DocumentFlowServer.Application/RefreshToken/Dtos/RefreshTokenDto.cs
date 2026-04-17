using System.Text.Json.Serialization;

namespace DocumentFlowServer.Application.RefreshToken.Dtos;

public class RefreshTokenDto
{
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
    
    [JsonPropertyName("expiresAt")]
    public DateTimeOffset? ExpiresAt { get; set; }
}