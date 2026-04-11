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
    Task<UserDto> GetUserByLoginAsync(string login);

    /// <summary>
    /// Проверяет наличие пользователя в таблице по почте
    /// </summary>
    Task<bool> IsUserAlreadyExists(string email);

    Task<List<UserDto>> GetAllUsersAsync(UserFilter filter);

    Task<PersonDto> GetPersonalInfo(int personId);

    Task<int> GetTotalCountAsync();
    
    Task<UserInfoDto> GetUserInfoByIdAsync(int userId);
}