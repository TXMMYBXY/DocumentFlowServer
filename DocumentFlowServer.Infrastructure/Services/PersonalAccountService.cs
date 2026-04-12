using AutoMapper;
using DocumentFlowServer.Application.Repository.Account;
using DocumentFlowServer.Application.Repository.User;
using DocumentFlowServer.Application.Services;
using DocumentFlowServer.Application.Services.Personal;
using DocumentFlowServer.Application.Services.Personal.Dto;
using DocumentFlowServer.Entities.Models;
using Microsoft.Extensions.Logging;

namespace DocumentFlowServer.Infrastructure.Services;

public class PersonalAccountService : IPersonalAccountService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<PersonalAccountService> _logger;
    private readonly IPasswordHasherService _passwordHasher;

    public PersonalAccountService(IMapper mapper,
        IUserRepository userRepository,
        IAccountRepository accountRepository,
        ILogger<PersonalAccountService> logger,
        IPasswordHasherService passwordHasher)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    public async Task<GetPersonDto> GetPersonalInfoAsync(int personId)
    {
        var person = await _userRepository.GetPersonalInfo(personId);
        var personDto = _mapper.Map<GetPersonDto>(person);

        return personDto;
    }

    public async Task ChangePasswordAsync(int personId, ChangePasswordDto changePasswordDto)
    {
        _logger.LogInformation("Changing password for user with ID {PersonId}", personId);
        
        var person = await _userRepository.GetByIdAsync(personId);

        ArgumentNullException.ThrowIfNull(person, "User not found");
        
        var currentPasswordStatus = _passwordHasher.Verify(person.PasswordHash, changePasswordDto.CurrentPassword);

        if(!currentPasswordStatus) 
            throw new InvalidOperationException("Current password is incorrect");
        
        var passwordsMatching = changePasswordDto.NewPassword.Equals(changePasswordDto.NewPassword);

        if (!passwordsMatching)
            throw new ArgumentException("Passwords not matched");

        person.PasswordHash = _passwordHasher.Hash(changePasswordDto.NewPassword);

        await _userRepository.SaveChangesAsync();

        _logger.LogInformation("Password changed successfully for user with ID {PersonId}", personId);
    }

    public async Task<List<GetLoginTimesDto>> GetLoginTimesAsync(int userId)
    {
        var loginTimes = await _accountRepository.GetLoginTimesByUserIdAsync(userId);
        var loginTimesDto = _mapper.Map<List<GetLoginTimesDto>>(loginTimes);

        return loginTimesDto;
    }

    public async Task AddNewLoginHistoryAsync(NewAuthRecordDto newAuthRecordDto)
    {
        _logger.LogInformation("Adding new login history for user with ID {UserId}", newAuthRecordDto.UserId);
        
        var loginHistory = _mapper.Map<LoginHistory>(newAuthRecordDto);

        var count = await _accountRepository.GetCountOfRecordsByUserIdAsync(newAuthRecordDto.UserId);

        if (count >= 10)
        {
            var lastLoginHistory = await _accountRepository.GetFirstLoginHistoryByUserIdAsync(newAuthRecordDto.UserId);

            await _accountRepository.DeleteAsync(lastLoginHistory);
        }

        await _accountRepository.AddNewLoginHistoryAsync(loginHistory);
        await _accountRepository.SaveChangesAsync();

        _logger.LogInformation("New login history added successfully for user with ID {UserId}", newAuthRecordDto.UserId);
    }
}