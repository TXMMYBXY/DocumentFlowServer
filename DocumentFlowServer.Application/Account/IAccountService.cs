using DocumentFlowServer.Application.Account.RequestDto;
using DocumentFlowServer.Application.Account.ResponseDto;

namespace DocumentFlowServer.Application.Account;

public interface IAccountService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto requestDto);
    Task<LoginRefreshResponseDto> LoginByRefreshTokenAsync(string refreshToken);
}