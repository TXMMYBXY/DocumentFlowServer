using DocumentFlowServer.Application.Account.RequestDto;
using DocumentFlowServer.Application.Account.ResponseDto;
using DocumentFlowServer.Application.Jwt.Dtos;
using DocumentFlowServer.Application.RefreshToken.Dtos;

namespace DocumentFlowServer.Application.Account;

public interface IAccountService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto requestDto);
    Task<LoginRefreshResponseDto> LoginByRefreshTokenAsync(string refreshToken);
    Task<AccessTokenDto> GetNewAccessTokenAsync(string refreshToken);
    Task<RefreshTokenDto> GetNewRefreshTokenAsync(string refreshToken);
    
}