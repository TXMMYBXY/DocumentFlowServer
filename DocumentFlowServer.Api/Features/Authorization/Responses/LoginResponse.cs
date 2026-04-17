using System.Text.Json.Serialization;
using DocumentFlowServer.Application.Jwt.Dtos;
using DocumentFlowServer.Application.RefreshToken.Dtos;
using DocumentFlowServer.Application.User.Dtos;

namespace DocumentFlowServer.Api.Features.Authorization.Responses;

public class LoginResponse
{
    [JsonPropertyName("access")]
    public AccessTokenDto Access { get; set; }
    
    [JsonPropertyName("refresh")]
    public RefreshTokenDto Refresh { get; set; }
    
    [JsonPropertyName("userInfo")]
    public UserInfoForLoginDto UserInfo { get; set; }
}