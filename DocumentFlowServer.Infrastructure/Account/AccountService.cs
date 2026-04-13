using AutoMapper;
using DocumentFlowServer.Application.Account;
using DocumentFlowServer.Application.Account.RequestDto;
using DocumentFlowServer.Application.Account.ResponseDto;
using DocumentFlowServer.Application.Common.Services;
using DocumentFlowServer.Application.Jwt;
using DocumentFlowServer.Application.Jwt.Dtos;
using DocumentFlowServer.Application.User;
using Microsoft.Extensions.Logging;

namespace DocumentFlowServer.Infrastructure.Account;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;

    public AccountService(
        ILogger<AccountService> logger,
        IMapper mapper,
        IPasswordHasher passwordHasher,
        IJwtService jwtService,
        IUserRepository userRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _userRepository = userRepository;
    }
    
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto requestDto)
    {
        _logger.LogInformation("login");
        
        var userLogin = await _userRepository.GetUserForLoginAsync(requestDto.Email);
        
        ArgumentNullException.ThrowIfNull(userLogin);

        Console.WriteLine(_passwordHasher.Hash(requestDto.Password));
        
        var isPasswordMatching = _passwordHasher.Verify(userLogin.PasswordHash, requestDto.Password);

        if (!isPasswordMatching)
        {
            throw new InvalidOperationException("Invalid password");
        }

        var userClaims = _mapper.Map<UserClaimsDto>(userLogin);
        
        var token = _jwtService.GenerateAccessToken(userClaims);
        
        return new LoginResponseDto
        {
            AccessToken = token
        };
    
    }
}