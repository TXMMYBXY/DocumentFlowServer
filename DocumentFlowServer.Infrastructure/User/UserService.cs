using AutoMapper;
using DocumentFlowServer.Application.Common.Services;
using DocumentFlowServer.Application.User;
using DocumentFlowServer.Application.User.Dtos;
using Microsoft.Extensions.Logging;

namespace DocumentFlowServer.Infrastructure.User;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    
    private readonly IUserRepository _userRepository;

    public UserService(
        ILogger<UserService> logger,
        IMapper mapper,
        IPasswordHasher passwordHasher,
        IUserRepository userRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }
    
    public async Task<PagedUserDto> GetUsersAsync(UserFilter filter)
    {
        var users = await _userRepository.GetAllUsers(filter);
        var totalCount = await _userRepository.GetCountAsync();

        return new PagedUserDto
        {
            Users = users,
            TotalCount = totalCount,
            PageSize = filter.PageSize ?? totalCount,
            CurrentPage = filter.PageNumber ?? 1
        };
    }

    public async Task CreateUserAsync(CreateUserDto createUserDto)
    {
        _logger.LogInformation("Creating new user");
        
        var user = _mapper.Map<Entities.Models.AboutUserModels.User>(createUserDto);
        
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        
        _logger.LogInformation("User successfully created");
    }

    public async Task UpdateUserInfoAsync(int userId, UpdateUserInfoDto updateUserInfoDto)
    {
        _logger.LogInformation("Updating user with id {UserId}", userId);
        
        var user = await _userRepository.GetByIdAsync(userId);
        
        ArgumentNullException.ThrowIfNull(user, "User not exists");
        
        _mapper.Map(updateUserInfoDto, user);
        
        await _userRepository.SaveChangesAsync();
        
        _logger.LogInformation("user with id {UserId} successfully updated", userId);
    }

    public async Task SetUserPasswordAsync(int userId, SetUserPasswordDto setUserPasswordDtoDto)
    {
        _logger.LogInformation("Setting new password for user with id {UserId}", userId);
        
        var userExists = await _userRepository.ExistsAsync(userId);
        
        if (!userExists)
        {
            throw new NullReferenceException("User not found");
        }
        
        await _userRepository.SetNewPasswordAsync(userId, _passwordHasher.Hash(setUserPasswordDtoDto.Password));
        
        _logger.LogInformation("Password changed successfully for user with id {UserId}", userId);
    }

    public async Task<bool> ChangeUserStatusAsync(int userId)
    {
        _logger.LogInformation("Updating status for user with id {UserId}", userId);
        
        var userExists = await _userRepository.ExistsAsync(userId);
        
        if (!userExists)
        {
            throw new NullReferenceException("User not found");
        }
        
        var userStatus = await _userRepository.ChangeUserStatusAsync(userId);
        
        _logger.LogInformation("Status changed successfully for user with id {UserId}", userId);
        
        return userStatus;
    }

    public async Task DeleteUserAsync(int userId)
    {
        _logger.LogInformation("Deleting user with id {UserId}", userId);
        
        var userExists = await _userRepository.ExistsAsync(userId);

        if (!userExists)
        {
            throw new NullReferenceException("User not found");
        }
        
        await _userRepository.DeleteAsync(userId);
        
        _logger.LogInformation("User with id {UserId} was deleted", userId);
    }
}