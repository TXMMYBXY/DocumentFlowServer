using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.Authorization.Responses;

public class AccessTokenResponse
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("expiresAt")]
    public string ExpiresAt { get; set; }
    
    [JsonPropertyName("tokenType")]
    public string TokenType { get; set; } = "Bearer";
}