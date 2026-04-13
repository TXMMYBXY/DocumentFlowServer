using System.Threading.Tasks;
using DocumentFlowServer.Application.User.Dtos;

namespace DocumentFlowServer.Application.User;

public interface IUserService
{
    Task<PagedUserDto> GetUsersAsync(UserFilter filter);
    Task CreateUserAsync(CreateUserDto createUserDto);
    Task SetUserPasswordAsync(int userId, SetUserPassword setUserPasswordDto);
    Task ChangeUserStatusAsync(int userId);
    Task DeleteUserAsync(int userId);
}