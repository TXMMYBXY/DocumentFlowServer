using DocumentFlowServer.Application.RefreshToken;
using DocumentFlowServer.Application.RefreshToken.Dtos;
using DocumentFlowServer.Infrastructure.Common.Repository;
using DocumentFlowServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.RefreshToken;

public class TokenRepository : BaseRepository<Entities.Models.AboutUserModels.RefreshToken>, ITokenRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public TokenRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RefreshTokenDto?> GetRefreshTokenByUserIdAsync(int userId)
    {
        return await _dbContext.RefreshTokens
            .Where(t => t.UserId == userId)
            .Select(t => new RefreshTokenDto
            {
                RefreshToken = t.Token,
                ExpiresAt = t.ExpiresAt,
            })
            .SingleOrDefaultAsync();
    }

    public async Task<RefreshTokenDto?> GetRefreshTokenByValueAsync(string token)
    {
        return await _dbContext.RefreshTokens
            .Where(t => t.Token.Equals(token))
            .Select(t => new RefreshTokenDto
            {
                RefreshToken = t.Token,
                ExpiresAt = t.ExpiresAt,
            })
            .SingleOrDefaultAsync();
    }

    public async Task RevokeRefreshTokenByUserIdAsync(int userId)
    {
        await _dbContext.RefreshTokens
            .Where(t => t.UserId == userId)
            .ExecuteDeleteAsync();
    }

    public async Task<int> GetRefreshTokenOwnerIdByValueAsync(string tokenHash)
    {
        return await _dbContext.RefreshTokens
            .Where(t => t.Token.Equals(tokenHash))
            .Select(t => t.UserId)
            .SingleOrDefaultAsync();
    }
}