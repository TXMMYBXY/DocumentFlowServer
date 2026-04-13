namespace DocumentFlowServer.Application.Department.Dtos;

public class UpdateDepartmentDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ICollection<int>? EmployeesIds { get; set; }
}