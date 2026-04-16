using DocumentFlowServer.Application.Jwt.Dtos;

namespace DocumentFlowServer.Application.Jwt;

public interface IJwtService
{
    AccessTokenDto GenerateAccessToken(UserClaimsDto userClaims);
}