using DocumentFlowServer.Application.Account;
using DocumentFlowServer.Application.Personal.Dtos;
using DocumentFlowServer.Entities.Models;
using DocumentFlowServer.Infrastructure.Common.Repository;
using DocumentFlowServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Account;

public class AccountRepository : BaseRepository<LoginHistory>, IAccountRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public AccountRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<LoginTimeDto>> GetLoginTimesByUserIdAsync(int userId)
    {
        return await _dbContext.LoginHistories
            .Where(l => l.UserId == userId)
            .Select(l => new LoginTimeDto
            {
                LoginTime = l.LoginDate
            })
            .OrderByDescending(l => l.LoginTime)
            .Take(10)
            .ToListAsync();
    }

    public async Task<int> GetCountOfRecordsByUserIdAsync(int userId)
    {
        return await _dbContext.LoginHistories
            .Where(l => l.UserId == userId)
            .CountAsync();
    }

    public async Task<int> GetFirstLoginHistoryByUserIdAsync(int userId)
    {
        return await _dbContext.LoginHistories
            .Where(l => l.UserId == userId)
            .Select(l => l.Id)
            .FirstOrDefaultAsync();
    }
}