using DocumentFlowServer.Application.Repository.Token.Dto;
using DocumentFlowServer.Entities.Models.AboutUserModels;

namespace DocumentFlowServer.Application.Repository.Token;

public interface ITokenRepository : IBaseRepository<RefreshToken>
{
    /// <summary>
    /// Добавляет токен обновления в таблицу
    /// </summary>
    System.Threading.Tasks.Task CreateRefreshTokenAsync(RefreshToken refreshToken);

    /// <summary>
    /// Возвращает токен обновления из таблицы по userId
    /// </summary>
    Task<RefreshTokenDto?> GetRefreshTokenByUserIdAsync(int userId);

    /// <summary>
    /// Возвращает токен обновления из таблицы по значению
    /// </summary>
    /// <param name="tokenValue"></param>
    /// <returns></returns>
    Task<RefreshTokenDto?> GetRefreshTokenByValueAsync(string tokenValue);
}
