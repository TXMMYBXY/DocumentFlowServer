using DocumentFlowServer.Application.User.Dtos;

namespace DocumentFlowServer.Application.RefreshToken.Dtos;

public class RefreshTokenDto
{
    public string Token { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public UserDto User { get; set; }
}