using DocumentFlowServer.Application.RefreshToken.Dtos;

namespace DocumentFlowServer.Application.RefreshToken;

public interface ITokenService
{
    Task<RefreshTokenDto> GenerateRefreshTokenAsync(int userId);
    Task<bool> IsValidRefreshToken(string refreshToken);
    Task<RefreshTokenDto> GetRefreshToken(string refreshToken);
    Task<int> GetRefreshTokenOwnerIdAsync(string refreshToken);
}