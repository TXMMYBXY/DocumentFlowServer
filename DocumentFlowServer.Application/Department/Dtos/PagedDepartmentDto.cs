using DocumentFlowServer.Application.Common;

namespace DocumentFlowServer.Application.Department.Dtos;

public class PagedDepartmentDto : PagedData
{
    public ICollection<DepartmentDto>? Departments { get; set; }
}