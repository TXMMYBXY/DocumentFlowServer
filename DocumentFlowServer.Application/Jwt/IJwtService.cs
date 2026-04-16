using DocumentFlowServer.Application.Jwt.Dtos;

namespace DocumentFlowServer.Application.Jwt;

/// <summary>
/// Service for creating jwt tokens
/// </summary>
public interface IJwtService
{
    AccessTokenDto GenerateAccessToken(UserClaimsDto userClaims);
}