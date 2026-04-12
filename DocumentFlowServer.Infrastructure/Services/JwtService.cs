using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DocumentFlowServer.Application.Repository.Token;
using DocumentFlowServer.Application.Repository.Token.Dto;
using DocumentFlowServer.Application.Repository.User.Dto;
using DocumentFlowServer.Application.Services.Authorization;
using DocumentFlowServer.Application.Services.RefreshTokenHasher;
using DocumentFlowServer.Entities.Models.AboutUserModels;
using DocumentFlowServer.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DocumentFlowServer.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly RefreshTokenSettings _refreshTokenSettings;
    private readonly ITokenRepository _tokenRepository;
    private readonly IRefreshTokenHasher _refreshTokenHashser;
    private readonly ILogger<JwtService> _logger;

    public JwtService(
        IOptions<JwtSettings> jwtSettings,
        IOptions<RefreshTokenSettings> refreshTokenSettings,
        ITokenRepository tokenRepository,
        IRefreshTokenHasher refreshTokenHasher,
        ILogger<JwtService> logger)
    {
        _jwtSettings = jwtSettings.Value;
        _refreshTokenSettings = refreshTokenSettings.Value;
        _tokenRepository = tokenRepository;
        _refreshTokenHashser = refreshTokenHasher;
        _logger = logger;
    }

    public string GenerateAccessToken(UserEntity user)
    {
        _logger.LogInformation("Generating access token for user with id {UserId}", user.Id);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        //Коллекция данных которые будут храниться в токене
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.Title),
            new Claim("RoleId", user.Role.Id.ToString()),
            new Claim("IsActive", user.IsActive.ToString()),
        };

        var jwtDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),                    // Утверждения (данные пользователя)
            Issuer = _jwtSettings.Issuer,                            // Издатель токена
            Audience = _jwtSettings.Audience,                        // Потребитель токена
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresMinutes), // Время истечения
            SigningCredentials = new SigningCredentials(           // Подпись токена
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(jwtDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        _logger.LogInformation("Access token generated successfully for user with id {UserId}", user.Id);

        return tokenString;
    }

    public async Task<RefreshToken> GenerateRefreshTokenAsync(int userId)
    {
        _logger.LogInformation("Generating refresh token for user with id {UserId}", userId);

        var targetToken = await _tokenRepository.GetRefreshTokenByUserIdAsync(userId);
        if (targetToken != null)
        {
            await _RevokeToken(targetToken);
        }

        var secretKey = _GenerateSecretLine();
        var refreshToken = new RefreshToken
        {
            Token = _refreshTokenHashser.Hash(secretKey),
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenSettings.ExpiresDays)
        };

        await _tokenRepository.CreateRefreshTokenAsync(refreshToken);
        await _tokenRepository.SaveChangesAsync();

        refreshToken.Token = secretKey;

        _logger.LogInformation("Refresh token generated successfully for user with id {UserId}", userId);

        return refreshToken;
    }
    
    public async Task<bool> ValidateRefreshTokenAsync(string refreshToken)
    {
        _logger.LogDebug("Starting validation of refresh token: {TokenHash}", refreshToken);
        
        var token = await _tokenRepository.GetRefreshTokenByValueAsync(_refreshTokenHashser.Hash(refreshToken));
        
        return token != null && token.ExpiresAt > DateTime.UtcNow;
    }

    public async Task<int> GetRefreshTokenOwnerAsync(string refreshToken)
    {
        var token = await _tokenRepository.GetRefreshTokenByValueAsync(_refreshTokenHashser.Hash(refreshToken));

        return token!.UserId;
    }

    /// <summary>
    /// Метод для генерации последовательности
    /// </summary>
    private string _GenerateSecretLine()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Метод для удаления токена из таблицы
    /// </summary>
    /// <param name="refreshToken"></param>
    private async Task _RevokeToken(RefreshTokenEntity refreshToken)
    {
        _logger.LogInformation("Revoking refresh token for user with id {UserId}", refreshToken.UserId);

        await _tokenRepository.DeleteAsync(refreshToken.Id);

        _logger.LogInformation("Refresh token revoked successfully for user with id {UserId}", refreshToken.UserId);
    }
}
