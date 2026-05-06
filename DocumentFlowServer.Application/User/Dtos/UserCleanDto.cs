using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Application.User.Dtos;

public class UserCleanDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public Role Role { get; set; }
}