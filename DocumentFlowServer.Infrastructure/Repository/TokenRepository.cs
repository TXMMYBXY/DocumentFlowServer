using DocumentFlowServer.Application.Repository.Token;
using DocumentFlowServer.Application.Repository.Token.Dto;
using DocumentFlowServer.Entities.Models.AboutUserModels;
using DocumentFlowServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Repository;

public class TokenRepository : BaseRepository<RefreshToken>, ITokenRepository
{
    private readonly ApplicationDbContext _dbContext;
    public TokenRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task CreateRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _dbContext.AddAsync(refreshToken);
    }

    public async Task<RefreshTokenEntity?> GetRefreshTokenByUserIdAsync(int userId)
    {
        return await _dbContext.RefreshTokens
            .Where(t => t.UserId == userId)
            .Select(t => new RefreshTokenEntity
            {
                Token = t.Token,
                ExpiresAt = t.ExpiresAt,
                UserId = t.UserId
            })
            .SingleOrDefaultAsync();
    }

    public async Task<RefreshTokenEntity?> GetRefreshTokenByValueAsync(string tokenValue)
    {
        return await _dbContext.RefreshTokens
            .Where(t => t.Token.Equals(tokenValue))
            .Select(t => new RefreshTokenEntity
            {
                Token = t.Token,
                ExpiresAt = t.ExpiresAt,
                UserId = t.UserId
            })
            .SingleOrDefaultAsync();
    }
}
