namespace DocumentFlowServer.Application.User.Dtos;

public class UserLoginDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public int RoleId { get; set; }
    public bool IsActive { get; set; }
    public string PasswordHash { get; set; }
}