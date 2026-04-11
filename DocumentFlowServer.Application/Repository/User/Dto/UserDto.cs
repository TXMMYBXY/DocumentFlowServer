using DocumentFlowServer.Application.Repository.Department.Dto;
using DocumentFlowServer.Application.Repository.Role.Dto;

namespace DocumentFlowServer.Application.Repository.User.Dto;

public class UserDto
{
    public int  Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public DepartmentDto Department { get; set; }
    public RoleDto Role { get; set; }
}