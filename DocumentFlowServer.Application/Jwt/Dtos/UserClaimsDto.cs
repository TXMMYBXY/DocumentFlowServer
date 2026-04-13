namespace DocumentFlowServer.Application.Jwt.Dtos;

public class UserClaimsDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string RoleId { get; set; }
    public string IsActive { get; set; }
}