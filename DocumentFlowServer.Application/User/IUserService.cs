using DocumentFlowServer.Application.User.Dtos;

namespace DocumentFlowServer.Application.User;

/// <summary>
/// Service for working with users
/// </summary>
public interface IUserService
{
    Task<PagedUserDto> GetUsersAsync(UserFilter filter);
    
    Task CreateUserAsync(CreateUserDto createUserDto);

    Task UpdateUserInfoAsync(int userId, UpdateUserInfoDto updateUserInfoDto);
    Task SetUserPasswordAsync(int userId, SetUserPasswordDto setUserPasswordDtoDto);
    Task<bool> ChangeUserStatusAsync(int userId);

    Task DeleteUserAsync(int userId);
    
    Task<UserLoginDto> GetUserInfoForLoginAsync(string email);
    Task<UserLoginDto> GetUserInfoByUserIdAsync(int userId);
    Task DeleteManyUsersAsync(List<int> requestUserIds);
}