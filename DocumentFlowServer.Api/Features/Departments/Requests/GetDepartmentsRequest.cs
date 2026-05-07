using DocumentFlowServer.Application.Department;

namespace DocumentFlowServer.Api.Features.Departments.Requests;

public class GetDepartmentsRequest
{
    public DepartmentSortField? SortBy { get; set; }
    public bool Descending { get; set; }
    
    public string? Title { get; set; }
    
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
}