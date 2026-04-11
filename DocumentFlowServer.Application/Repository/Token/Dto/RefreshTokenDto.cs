using System.ComponentModel.DataAnnotations.Schema;
using DocumentFlowServer.Application.Repository.User.Dto;

namespace DocumentFlowServer.Application.Repository.Token.Dto;

public class RefreshTokenDto
{
    public string? Token { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public UserDto User { get; set; }
}