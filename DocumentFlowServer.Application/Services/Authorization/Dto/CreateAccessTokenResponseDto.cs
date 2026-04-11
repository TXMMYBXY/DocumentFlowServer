namespace DocumentFlowServer.Application.Services.Authorization.Dto;

public class CreateAccessTokenResponseDto
{
    public UserInfoForLoginDto UserInfo { get; set; }
    public string AccessToken { get; set; }
    public string ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
}
