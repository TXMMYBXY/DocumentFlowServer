using System.ComponentModel.DataAnnotations.Schema;
using DocumentFlowServer.Application.Repository.User.Dto;

namespace DocumentFlowServer.Application.Repository.Token.Dto;

public class RefreshTokenEntity
{
    public int Id { get; set; }
    public string? Token { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public UserEntity User { get; set; }
}