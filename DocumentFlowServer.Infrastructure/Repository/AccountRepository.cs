using DocumentFlowServer.Application.Repository.Account;
using DocumentFlowServer.Application.Repository.Account.Dto;
using DocumentFlowServer.Entities.Models;
using DocumentFlowServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Repository;

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
            .Select(l => new LoginTimeDto()
            {
                LoginTime = l.LoginDate
            })
            .OrderByDescending(l => l.LoginTime)
            .Take(10)
            .ToListAsync();
    }

    public async Task AddNewLoginHistoryAsync(LoginHistory loginHistory)
    {
        await _dbContext.LoginHistories.AddAsync(loginHistory);
    }

    public async Task<int> GetCountOfRecordsByUserIdAsync(int userId)
    {
        return await _dbContext.LoginHistories.CountAsync(l => l.UserId == userId);
    }

    public async Task<int> GetFirstLoginHistoryByUserIdAsync(int userId)
    {
        return await _dbContext.LoginHistories
            .Where(l => l.UserId == userId)
            .Select(l => l.Id)
            .FirstOrDefaultAsync();
    }
}