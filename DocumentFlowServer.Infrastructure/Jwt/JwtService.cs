using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DocumentFlowServer.Application.Common.Configuration;
using DocumentFlowServer.Application.Jwt;
using DocumentFlowServer.Application.Jwt.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DocumentFlowServer.Infrastructure.Jwt;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly  ILogger<JwtService> _logger;

    public JwtService(
        IOptions<JwtSettings> jwtSettings,
        ILogger<JwtService> logger)
    {
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }
    
    public AccessTokenDto GenerateAccessToken(UserClaimsDto userClaims)
    {
        _logger.LogInformation("Generating access token for user with id {UserId}", userClaims.Id);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, userClaims.Id.ToString()),
            new (ClaimTypes.Email, userClaims.Email),
            new (ClaimTypes.Role, userClaims.Role),
            new ("RoleId", userClaims.RoleId),
            new ("IsActive", userClaims.IsActive)
        };

        var jwtDescriptor = new SecurityTokenDescriptor
        {
            Audience = _jwtSettings.Audience,
            Subject = new ClaimsIdentity(claims),
            Issuer = _jwtSettings.Issuer,
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresMinutes),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        
        var token = tokenHandler.CreateToken(jwtDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        
        return new AccessTokenDto
        {
            AccessToken = tokenString,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresMinutes)
        };
    }
}