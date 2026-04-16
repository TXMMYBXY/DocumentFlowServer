using System.Text.Json.Serialization;
using DocumentFlowServer.Application.User.Dtos;

namespace DocumentFlowServer.Api.Features.Authorization.Responses;

public class AccessTokenResponse
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("expiresAt")]
    public string ExpiresAt { get; set; }
    
    [JsonPropertyName("tokenType")]
    public string TokenType { get; set; } = "Bearer";
    
    [JsonPropertyName("userInfo")]
    public UserInfoForLoginDto UserInfo { get; set; }
}