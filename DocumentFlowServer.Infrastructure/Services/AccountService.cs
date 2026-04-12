using AutoMapper;
using DocumentFlowServer.Application.Repository.Token;
using DocumentFlowServer.Application.Repository.User;
using DocumentFlowServer.Application.Services;
using DocumentFlowServer.Application.Services.Authorization;
using DocumentFlowServer.Application.Services.Authorization.Dto;
using DocumentFlowServer.Application.Services.Personal;
using DocumentFlowServer.Application.Services.Personal.Dto;
using DocumentFlowServer.Application.Services.RefreshTokenHasher;
using DocumentFlowServer.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DocumentFlowServer.Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;
    private readonly IRefreshTokenHasher _refreshTokenHasher;
    private readonly IPersonalAccountService _personalAccountService;
    private readonly ILogger<AccountService> _logger;
    private readonly IPasswordHasherService _passwordHasher;

    public AccountService(
        IMapper mapper,
        IUserRepository userRepository,
        ITokenRepository tokenRepository,
        IJwtService jwtService,
        IOptions<JwtSettings> jwtSettings,
        IRefreshTokenHasher refreshTokenHasher,
        IPersonalAccountService personalAccountService,
        ILogger<AccountService> logger,
        IPasswordHasherService passwordHasher)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
        _refreshTokenHasher = refreshTokenHasher;
        _personalAccountService = personalAccountService;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginUserDto loginUserDto)
    {
        _logger.LogInformation("Attempting to log in user with email: {Email}", loginUserDto.Email);

        var user = await _userRepository.GetUserByLoginAsync(loginUserDto.Email);

        ArgumentNullException.ThrowIfNull("Incorrect login");

        if (!user.IsActive)
            throw new ArgumentException("User was blocked");

        var result = _passwordHasher.Verify(user.PasswordHash, loginUserDto.PasswordHash);

        if(result)
            throw new ArgumentException("Incorrect password");

        await _personalAccountService.AddNewLoginHistoryAsync(new NewAuthRecordDto { UserId = user.Id });

        var loginResponseDto = new LoginResponseDto
        {
            UserInfo = _mapper.Map<UserInfoForLoginDto>(user),
            AccessToken = _jwtService.GenerateAccessToken(user),
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresMinutes).ToString(),
            RefreshToken = _mapper.Map<RefreshTokenDto>(await _jwtService.GenerateRefreshTokenAsync(user.Id))
        };

        _logger.LogInformation("User with email: {Email} logged in successfully", loginUserDto.Email);
        
        return loginResponseDto;
    }

    public async Task<CreateAccessTokenResponseDto> CreateAccessTokenAsync(string refreshToken)
    {
        _logger.LogInformation("Attempting to create access token");
        
        var isValid = await _jwtService.ValidateRefreshTokenAsync(refreshToken);

        if (!isValid)
            throw new NullReferenceException("Incorrect token");

        var user = await _userRepository.GetUserByIdAsync(await _jwtService.GetRefreshTokenOwnerAsync(refreshToken));
        
        ArgumentNullException.ThrowIfNull("Incorrect user");
        
        var accessTokenResponseDto = new CreateAccessTokenResponseDto
        {
            UserInfo = _mapper.Map<UserInfoForLoginDto>(user),
            AccessToken = _jwtService.GenerateAccessToken(user),
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresMinutes).ToString()
        };
        
        _logger.LogInformation("Access token created successfully for user ID: {UserId}", user.Id);
        
        return accessTokenResponseDto;
    }

    public async Task<RefreshTokenResponseDto> CreateRefreshTokenAsync(string refreshToken)
    {
        _logger.LogInformation("Attempting to create refresh token");
        
        var isValid = await _jwtService.ValidateRefreshTokenAsync(refreshToken);

        if (!isValid)
            throw new NullReferenceException("Incorrect token");

        var userId = await _jwtService.GetRefreshTokenOwnerAsync(refreshToken);
        
        var token = _mapper.Map<RefreshTokenDto>(await _jwtService.GenerateRefreshTokenAsync(userId));
        var refreshTokenResponseDto = _mapper.Map<RefreshTokenResponseDto>(token);

        _logger.LogInformation("Refresh token created successfully for user ID: {UserId}", userId);
        
        return refreshTokenResponseDto;
    }

    public async Task<RefreshTokenToLoginResponseDto> LoginByRefreshTokenAsync(RefreshTokenToLoginDto refreshToken)
    {
        var token = await _tokenRepository.GetRefreshTokenByValueAsync(_refreshTokenHasher.Hash(refreshToken.RefreshToken));

        ArgumentNullException.ThrowIfNull("Incorrect token");

        _logger.LogInformation("Attempting to log in by refresh token for user ID: {UserId}", token.UserId);

        if(token.ExpiresAt < DateTime.UtcNow)
            throw new InvalidOperationException("Refresh token has expired at " + token.ExpiresAt);

        await _personalAccountService.AddNewLoginHistoryAsync(new NewAuthRecordDto { UserId = token.UserId });

        var response = new RefreshTokenToLoginResponseDto
        {
            IsAllowed = true
        };

        _logger.LogInformation("User with ID: {UserId} logged in successfully by refresh token", token.UserId);

        return response;
    }
}
