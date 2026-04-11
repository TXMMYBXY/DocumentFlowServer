using DocumentFlowServer.Application.Repository.Account.Dto;
using DocumentFlowServer.Entities.Models;

namespace DocumentFlowServer.Application.Repository.Account;

public interface IAccountRepository : IBaseRepository<LoginHistory>
{
    Task<List<LoginTimeDto>> GetLoginTimesByUserIdAsync(int userId);
    System.Threading.Tasks.Task AddNewLoginHistoryAsync(LoginHistory loginHistory);
    Task<int> GetCountOfRecordsByUserIdAsync(int userId);
    Task<int> GetFirstLoginHistoryByUserIdAsync(int userId);
}