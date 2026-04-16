using DocumentFlowServer.Application.Common.Reposiroty;
using DocumentFlowServer.Application.User.Dtos;

namespace DocumentFlowServer.Application.User;

/// <summary>
/// Repository for working with users in database
/// </summary>
public interface IUserRepository : IBaseRepository<Entities.Models.AboutUserModels.User>
{
    Task<ICollection<UserDto>> GetAllUsers(UserFilter filter);
    
    Task<bool> ChangeUserStatusAsync(int userId);
    Task SetNewPasswordAsync(int userId, string hash);

    Task<UserLoginDto?> GetUserForLoginAsync(string email);
    Task<UserLoginDto?> GetUserForAccessAsync(int userId);
}