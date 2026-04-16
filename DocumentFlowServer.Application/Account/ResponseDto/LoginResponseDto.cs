using DocumentFlowServer.Application.Jwt.Dtos;
using DocumentFlowServer.Application.RefreshToken.Dtos;
using DocumentFlowServer.Application.User.Dtos;

namespace DocumentFlowServer.Application.Account.ResponseDto;

public class LoginResponseDto
{
    public AccessTokenDto AccessToken { get; set; }
    public RefreshTokenDto RefreshToken { get; set; }
    public UserInfoForLoginDto UserInfo { get; set; }
}