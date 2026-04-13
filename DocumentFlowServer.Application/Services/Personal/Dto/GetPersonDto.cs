using DocumentFlowServer.Application.Services.Role.Dto;

namespace DocumentFlowServer.Application.Services.Personal.Dto;

public class GetPersonDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Department { get; set; }
    public RoleDto Role { get; set; }
}