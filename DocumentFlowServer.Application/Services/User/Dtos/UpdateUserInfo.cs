namespace DocumentFlowServer.Application.Services.User.Dtos;

public class UpdateUserInfo
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public int DepartmentId { get; set; }
    public int RoleId { get; set; }
}