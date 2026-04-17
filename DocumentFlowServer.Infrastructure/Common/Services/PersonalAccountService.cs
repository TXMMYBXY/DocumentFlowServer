using AutoMapper;
using DocumentFlowServer.Application.Account;
using DocumentFlowServer.Application.Common.Services;
using DocumentFlowServer.Application.Personal;
using DocumentFlowServer.Application.Personal.Dtos;
using DocumentFlowServer.Application.User;
using DocumentFlowServer.Entities.Models;
using Microsoft.Extensions.Logging;

namespace DocumentFlowServer.Infrastructure.Common.Services;

public class PersonalAccountService : IPersonalAccountService
{
    private readonly ILogger<PersonalAccountService> _logger;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;

    public PersonalAccountService(
        ILogger<PersonalAccountService> logger,
        IMapper mapper,
        IPasswordHasher passwordHasher,
        IAccountRepository accountRepository,
        IUserRepository userRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _accountRepository = accountRepository;
        _userRepository = userRepository;
    }
    
    public async Task<PersonDto> GetPersonalInfoAsync(int personId)
    {
        _logger.LogInformation("getting personal info");

        return await _userRepository.GetCurrentUserByIdAsync(personId);
    }

    public async Task ChangePasswordAsync(int personId, ChangePasswordDto changePasswordDto)
    {
        _logger.LogInformation("Changing password for user with ID {PersonId}", personId);
        
        var person = await _userRepository.GetByIdAsync(personId);

        var currentPasswordStatus = _passwordHasher.Verify(person.PasswordHash, changePasswordDto.CurrentPassword);
        
        if(!currentPasswordStatus)
            throw new InvalidOperationException("Current password doesn't match");
        
        var passwordsMatching = changePasswordDto.NewPassword.Equals(changePasswordDto.NewPassword);
        
        if(!passwordsMatching)
            throw new InvalidOperationException("Passwords do not match");

        person.PasswordHash = _passwordHasher.Hash(changePasswordDto.NewPassword);

        await _userRepository.SaveChangesAsync();

        _logger.LogInformation("Password changed successfully for user with ID {PersonId}", personId);
    }

    public async Task<List<LoginTimeDto>> GetLoginTimesAsync(int userId)
    {
        _logger.LogInformation("getting login times");
        
        return await _accountRepository.GetLoginTimesByUserIdAsync(userId);
    }

    public async Task AddNewLoginHistoryAsync(AuthRecordDto authRecordDto)
    {
        _logger.LogInformation("Adding new login history for user with ID {UserId}", authRecordDto.UserId);
        
        var loginHistory = _mapper.Map<LoginHistory>(authRecordDto);

        var count = await _accountRepository.GetCountOfRecordsByUserIdAsync(authRecordDto.UserId);

        if (count >= 10)
        {
            var lastLoginHistory = await _accountRepository.GetFirstLoginHistoryByUserIdAsync(authRecordDto.UserId);

            await _accountRepository.DeleteAsync(lastLoginHistory);
        }

        await _accountRepository.AddAsync(loginHistory);
        await _accountRepository.SaveChangesAsync();

        _logger.LogInformation("New login history added successfully for user with ID {UserId}", authRecordDto.UserId);
    }
}