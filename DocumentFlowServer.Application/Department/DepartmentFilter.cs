namespace DocumentFlowServer.Application.Department;

public class DepartmentFilter
{
    public DepartmentSortField? SortBy { get; set; }
    public bool Descending { get; set; }
    
    public string? Title { get; set; }
    
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
}