namespace DocumentFlowServer.Application.Services.Authorization.Dto;

public class RefreshTokenResponseDto
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int UserId { get; set; }
}
