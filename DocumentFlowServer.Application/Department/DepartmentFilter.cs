namespace DocumentFlowServer.Application.Department;

public class DepartmentFilter
{
    public string? Title { get; set; }
    
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
}