using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentFlowServer.Application.Common.Reposiroty;
using DocumentFlowServer.Application.User.Dtos;

namespace DocumentFlowServer.Application.User;

public interface IUserRepository : IBaseRepository<Entities.Models.AboutUserModels.User>
{
    Task<ICollection<UserDto>> GetAllUsers(UserFilter filter);
    
    Task<bool> ChangeUserStatusAsync(int userId);
    Task SetNewPasswordAsync(int userId, string hash);

    Task<UserLoginDto?> GetUserForLoginAsync(string email);
}