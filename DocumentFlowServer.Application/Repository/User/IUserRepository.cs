using DocumentFlowServer.Application.Repository.User.Dto;
using DocumentFlowServer.Application.Services.User;

namespace DocumentFlowServer.Application.Repository.User;

public interface IUserRepository : IBaseRepository<Entities.Models.AboutUserModels.User>
{
    /// <summary>
    /// Обновляет статус пользователя
    /// </summary>
    Task<bool> UpdateUserStatusAsync(int userId);

    /// <summary>
    /// Возвращает пользователя из таблицы по почте
    /// </summary>
    Task<UserEntity?> GetUserByLoginAsync(string login);

    /// <summary>
    /// Проверяет наличие пользователя в таблице по почте
    /// </summary>
    Task<bool> IsUserAlreadyExists(string email);

    Task<List<UserEntity>> GetAllUsersAsync(UserFilter filter);

    Task<PersonDto?> GetPersonalInfo(int personId);

    Task<UserInfoDto?> GetUserInfoByIdAsync(int userId);

    Task<UserEntity?> GetUserByIdAsync(int userId);
}