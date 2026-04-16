using System.Security.Cryptography;
using AutoMapper;
using DocumentFlowServer.Application.Common.Configuration;
using DocumentFlowServer.Application.RefreshToken;
using DocumentFlowServer.Application.RefreshToken.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DocumentFlowServer.Infrastructure.RefreshToken;

public class TokenService : ITokenService
{
    private readonly RefreshTokenSettings _refreshTokenSettings;
    
    private readonly ILogger<TokenService> _logger;
    private readonly IMapper _mapper;
    private readonly IRefreshTokenHasher _refreshTokenHasher;
    private readonly ITokenRepository _tokenRepository;

    public TokenService(
        IOptions<RefreshTokenSettings> refreshTokenSettings,
        ILogger<TokenService> logger,
        IMapper mapper,
        IRefreshTokenHasher refreshTokenHasher,
        ITokenRepository tokenRepository)
    {
        _refreshTokenSettings = refreshTokenSettings.Value;
        _logger = logger;
        _mapper = mapper;
        _refreshTokenHasher = refreshTokenHasher;
        _tokenRepository = tokenRepository;
    }
    
    public async Task<RefreshTokenDto> GenerateRefreshTokenAsync(int userId)
    {
        var targetToken = await _tokenRepository.GetRefreshTokenByUserIdAsync(userId);
        
        if (targetToken != null)
            await _tokenRepository.RevokeRefreshTokenByUserIdAsync(userId);
        

        var secretKey = _GenerateSecretLine();
        var refreshToken = new Entities.Models.AboutUserModels.RefreshToken
        {
            Token = _refreshTokenHasher.Hash(secretKey),
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenSettings.ExpiresDays)
        };

        await _tokenRepository.AddAsync(refreshToken);
        await _tokenRepository.SaveChangesAsync();

        refreshToken.Token = secretKey;

        _logger.LogInformation("Refresh token generated successfully for user with id {UserId}", userId);
        
        return _mapper.Map<RefreshTokenDto>(refreshToken);
    }

    public async Task<bool> IsValidRefreshToken(string refreshToken)
    {
        _logger.LogInformation("Validating refresh token {RefreshToken}", refreshToken);
        
        var token = await _tokenRepository.GetRefreshTokenByValueAsync(_refreshTokenHasher.Hash(refreshToken));
        
        if (token == null)
            return false;

        if (token.ExpiresAt < DateTime.UtcNow)
            return false;
        
        _logger.LogInformation("Validated refresh token {RefreshToken}", refreshToken);
        
        return true;
    }

    public async Task<RefreshTokenDto> GetRefreshToken(string refreshToken)
    {
        var refreshTokenDto = await _tokenRepository.GetRefreshTokenByValueAsync(_refreshTokenHasher.Hash(refreshToken));
        
        refreshTokenDto.Token = refreshToken;
        
        ArgumentNullException.ThrowIfNull(refreshTokenDto, "Refresh token is no valid");
        
        return refreshTokenDto;
    }

    private static string _GenerateSecretLine()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        
        return Convert.ToBase64String(bytes);
    }
}