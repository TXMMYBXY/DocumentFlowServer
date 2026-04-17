using DocumentFlowServer.Application.Role.Dtos;

namespace DocumentFlowServer.Application.Personal.Dtos;

public class PersonDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Department { get; set; }
    public RoleDto Role { get; set; }
}