namespace DocumentFlowServer.Application.Department.Dtos;

/// <summary>
/// Dto без сотрудников
/// </summary>
public class DepartmentCleanDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
}