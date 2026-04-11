using DocumentFlowServer.Application.Services.Role.Dto;

namespace DocumentFlowServer.Application.Services.Authorization.Dto;

public class UserInfoForLoginDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }
    public virtual RoleDto Role { get; set; }
    public string Department { get; set; }
}
