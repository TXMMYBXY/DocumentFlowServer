using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Application.Jwt.Dtos;

public class UserClaimsDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    public string IsActive { get; set; }
}