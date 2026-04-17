using System.Text.Json.Serialization;
using DocumentFlowServer.Application.RefreshToken.Dtos;

namespace DocumentFlowServer.Api.Features.Authorization.Responses;

public class LoginRefreshResponse
{
    [JsonPropertyName("isAllowed")]
    public bool IsAllowed { get; set; }
    
    [JsonPropertyName("refreshToken")]
    public RefreshTokenDto RefreshToken { get; set; }
}