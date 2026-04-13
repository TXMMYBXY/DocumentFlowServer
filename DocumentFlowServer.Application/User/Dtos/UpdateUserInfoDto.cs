namespace DocumentFlowServer.Application.User.Dtos;

public class UpdateUserInfoDto
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public int? DepartmentId { get; set; }
    public int? RoleId { get; set; }
}