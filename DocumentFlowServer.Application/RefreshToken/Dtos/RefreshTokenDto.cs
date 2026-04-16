namespace DocumentFlowServer.Application.RefreshToken.Dtos;

public class RefreshTokenDto
{
    public string Token { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }
}