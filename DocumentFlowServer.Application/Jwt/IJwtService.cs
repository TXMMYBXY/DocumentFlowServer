using DocumentFlowServer.Application.Jwt.Dtos;

namespace DocumentFlowServer.Application.Jwt;

public interface IJwtService
{
    string GenerateAccessToken(UserClaimsDto userClaims);
}