namespace DocumentFlowServer.Application.Services.Authorization.Dto;

public class LoginResponseDto
{
    public UserInfoForLoginDto UserInfo { get; set; }
    public string AccessToken { get; set; }
    /// <summary>
    /// Время жизни токена доступа
    /// </summary>
    public string ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public RefreshTokenDto RefreshToken { get; set; }
}
