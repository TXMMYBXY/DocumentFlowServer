using AutoMapper;
using DocumentFlowServer.Application.Account;
using DocumentFlowServer.Application.Account.RequestDto;
using DocumentFlowServer.Application.Account.ResponseDto;
using DocumentFlowServer.Application.Common.Services;
using DocumentFlowServer.Application.Jwt;
using DocumentFlowServer.Application.Jwt.Dtos;
using DocumentFlowServer.Application.RefreshToken;
using DocumentFlowServer.Application.RefreshToken.Dtos;
using DocumentFlowServer.Application.User;
using DocumentFlowServer.Application.User.Dtos;
using Microsoft.Extensions.Logging;

namespace DocumentFlowServer.Infrastructure.Account;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AccountService(
        ILogger<AccountService> logger,
        IMapper mapper,
        IPasswordHasher passwordHasher,
        IJwtService jwtService,
        IUserService userService,
        ITokenService tokenService)
    {
        _logger = logger;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _userService = userService;
        _tokenService = tokenService;
    }
    
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto requestDto)
    {
        _logger.LogInformation("login");
        
        var userLogin = await _userService.GetUserInfoForLoginAsync(requestDto.Email);
        
        ArgumentNullException.ThrowIfNull(userLogin);

        Console.WriteLine(_passwordHasher.Hash(requestDto.Password));
        
        var isPasswordMatching = _passwordHasher.Verify(userLogin.PasswordHash, requestDto.Password);

        if (!isPasswordMatching)
        {
            throw new InvalidOperationException("Invalid password");
        }

        var userClaims = _mapper.Map<UserClaimsDto>(userLogin);
        
        var accessToken = _jwtService.GenerateAccessToken(userClaims);
        var refreshToken = await _tokenService.GenerateRefreshTokenAsync(userLogin.Id);
        var userInfoDto = _mapper.Map<UserInfoForLoginDto>(userLogin);
        
        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            UserInfo = userInfoDto
        };
    }

    public async Task<LoginRefreshResponseDto> LoginByRefreshTokenAsync(string refreshToken)
    {
        _logger.LogInformation("login by refresh token");

        var isTokenValid = await _tokenService.IsValidRefreshToken(refreshToken);

        if (!isTokenValid)
            throw new ArgumentNullException("Refresh token is no valid");

        var refreshTokenDto = await _tokenService.GetRefreshToken(refreshToken);

        return new LoginRefreshResponseDto
        {
            IsAllowed = true,
            RefreshToken = refreshTokenDto
        };
    }

    public async Task<AccessTokenDto> GetNewAccessTokenAsync(string refreshToken)
    {
        var userId = await _tokenService.GetRefreshTokenOwnerIdAsync(refreshToken);
        var userInfo = await _userService.GetUserInfoByUserIdAsync(userId);

        var userClaims = _mapper.Map<UserClaimsDto>(userInfo);
        
        var accessTokenDto = _jwtService.GenerateAccessToken(userClaims);

        return accessTokenDto;
    }

    public async Task<RefreshTokenDto> GetNewRefreshTokenAsync(string refreshToken)
    {
        var userId = await _tokenService.GetRefreshTokenOwnerIdAsync(refreshToken);

        var refreshTokenDto = await _tokenService.GenerateRefreshTokenAsync(userId);

        return refreshTokenDto;
    }
}