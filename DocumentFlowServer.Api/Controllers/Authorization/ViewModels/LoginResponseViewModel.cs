using DocumentFlowServer.Application.Services.Authorization.Dto;
using RefreshTokenDto = DocumentFlowServer.Application.Repository.Token.Dto.RefreshTokenDto;

namespace DocumentFlowServer.Api.Controllers.Authorization.ViewModels;

public class LoginResponseViewModel
{
    public UserInfoForLoginDto UserInfo { get; set; }
    public string AccessToken { get; set; }
    public string ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public RefreshTokenDto RefreshToken { get; set; }
}
