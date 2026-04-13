using DocumentFlowServer.Application.Common;

namespace DocumentFlowServer.Application.Services.Department.Dtos;

public class PagedDepartmentDto : PagedData
{
    public ICollection<DepartmentDto>? Departments { get; set; }
}