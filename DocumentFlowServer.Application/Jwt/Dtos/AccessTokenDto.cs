namespace DocumentFlowServer.Application.Jwt.Dtos;

public class AccessTokenDto
{
    public string AccessToken { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
}