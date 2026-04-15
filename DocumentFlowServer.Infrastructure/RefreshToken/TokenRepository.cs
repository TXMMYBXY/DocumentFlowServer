using DocumentFlowServer.Application.Department.Dtos;
using DocumentFlowServer.Application.RefreshToken;
using DocumentFlowServer.Application.RefreshToken.Dtos;
using DocumentFlowServer.Application.Role.Dtos;
using DocumentFlowServer.Application.User.Dtos;
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

    public async Task<RefreshTokenDto>? GetRefreshTokenByUserIdAsync(int userId)
    {
        return await _dbContext.RefreshTokens
            .Where(t => t.UserId == userId)
            .Select(t => new RefreshTokenDto
            {
                Token = t.Token,
                ExpiresAt = t.ExpiresAt,
                User = new UserDto
                {
                    Id = t.User.Id,
                    Email = t.User.Email,
                    Department = new DepartmentCleanDto
                    {
                        Id = t.User.Department.Id,
                        Title = t.User.Department.Title
                    },
                    Role = new RoleDto
                    {
                        Id = t.User.Role.Id,
                        Title = t.User.Role.Title
                    },
                    IsActive = t.User.IsActive
                }
            })
            .SingleOrDefaultAsync();
    }

    public async Task<RefreshTokenDto> GetRefreshTokenByValueAsync(string token)
    {
        return await _dbContext.RefreshTokens
            .Where(t => t.Token.Equals(token))
            .Select(t => new RefreshTokenDto
            {
                Token = t.Token,
                ExpiresAt = t.ExpiresAt,
                User = new UserDto
                {
                    Id = t.User.Id,
                    Email = t.User.Email,
                    Department = new DepartmentCleanDto
                    {
                        Id = t.User.Department.Id,
                        Title = t.User.Department.Title
                    },
                    Role = new RoleDto
                    {
                        Id = t.User.Role.Id,
                        Title = t.User.Role.Title
                    },
                    IsActive = t.User.IsActive
                }
            })
            .SingleOrDefaultAsync();
    }

    public async Task RevokeRefreshTokenByUserIdAsync(int userId)
    {
        await _dbContext.RefreshTokens
            .Where(t => t.UserId == userId)
            .ExecuteDeleteAsync();
    }
}