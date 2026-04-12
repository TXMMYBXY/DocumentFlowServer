using DocumentFlowServer.Application.Repository.Department.Dto;
using DocumentFlowServer.Application.Repository.Role.Dto;

namespace DocumentFlowServer.Application.Repository.User.Dto;

public class UserEntity
{
    public int  Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool IsActive { get; set; }
    public DepartmentEntity Department { get; set; }
    public RoleEntity Role { get; set; }
}