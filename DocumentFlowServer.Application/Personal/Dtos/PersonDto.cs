using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Application.Personal.Dtos;

public class PersonDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Department { get; set; }
    public Role Role { get; set; }
}