using DocumentFlowServer.Application.Repository.User.Dto;

namespace DocumentFlowServer.Application.Repository.Department.Dto;

public class DepartmentEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public ICollection<UserInfoDto>? Employees { get; set; }
}