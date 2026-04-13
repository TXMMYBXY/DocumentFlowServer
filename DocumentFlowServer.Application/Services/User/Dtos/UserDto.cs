namespace DocumentFlowServer.Application.Services.User.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool IsActive { get; set; }
    public int DepartmentId { get; set; }
    public int RoleId { get; set; }
}