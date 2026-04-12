using DocumentFlowServer.Application.Repository.Token.Dto;
using DocumentFlowServer.Application.Services.Authorization.Dto;

namespace DocumentFlowServer.Api.Controllers.Authorization.ViewModels;

public class LoginResponseViewModel
{
    public UserInfoForLoginDto UserInfo { get; set; }
    public string AccessToken { get; set; }
    public string ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public RefreshTokenEntity RefreshToken { get; set; }
}
