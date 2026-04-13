namespace DocumentFlowServer.Application.Services.User.Dto;

public class GetUserDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public string Department { get; set; }
    public virtual Entities.Models.AboutUserModels.Role Role { get; set; }
}