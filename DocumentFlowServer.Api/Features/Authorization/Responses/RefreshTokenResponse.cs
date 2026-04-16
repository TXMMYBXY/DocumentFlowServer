using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.Authorization.Responses;

public class RefreshTokenResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
    
    [JsonPropertyName("expiresAt")]
    public DateTimeOffset ExpiresAt { get; set; }
}