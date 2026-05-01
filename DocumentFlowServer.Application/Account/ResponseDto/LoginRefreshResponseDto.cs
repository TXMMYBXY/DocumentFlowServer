using DocumentFlowServer.Application.Jwt.Dtos;
using DocumentFlowServer.Application.RefreshToken.Dtos;

namespace DocumentFlowServer.Application.Account.ResponseDto;

public class LoginRefreshResponseDto
{
    public bool IsAllowed { get; set; }
    public AccessTokenDto AccessToken { get; set; }
    public RefreshTokenDto RefreshToken { get; set; }
}