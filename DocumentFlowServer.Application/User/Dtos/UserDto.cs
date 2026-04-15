using DocumentFlowServer.Application.Department.Dtos;
using DocumentFlowServer.Application.Role.Dtos;

namespace DocumentFlowServer.Application.User.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public DepartmentCleanDto Department { get; set; }
    public RoleDto Role { get; set; }
}