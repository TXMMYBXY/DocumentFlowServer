using DocumentFlowServer.Application.Common.Reposiroty;
using DocumentFlowServer.Application.RefreshToken.Dtos;

namespace DocumentFlowServer.Application.RefreshToken;

public interface ITokenRepository : IBaseRepository<Entities.Models.AboutUserModels.RefreshToken>
{
    Task<RefreshTokenDto>? GetRefreshTokenByUserIdAsync(int userId);
    Task<RefreshTokenDto> GetRefreshTokenByValueAsync(string token);

    Task RevokeRefreshTokenByUserIdAsync(int userId);
}