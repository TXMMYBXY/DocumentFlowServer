using DocumentFlowServer.Application.Common.Reposiroty;
using DocumentFlowServer.Application.Personal.Dtos;
using DocumentFlowServer.Entities.Models;

namespace DocumentFlowServer.Application.Account;

public interface IAccountRepository : IBaseRepository<LoginHistory>
{
    Task<List<LoginTimeDto>> GetLoginTimesByUserIdAsync(int userId);
    Task<int> GetCountOfRecordsByUserIdAsync(int userId);
    Task<int> GetFirstLoginHistoryByUserIdAsync(int userId);
}