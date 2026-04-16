using DocumentFlowServer.Application.Common.Reposiroty;
using DocumentFlowServer.Application.Department.Dtos;

namespace DocumentFlowServer.Application.Department;

/// <summary>
/// Repository for working with departments in database
/// </summary>
public interface IDepartmentRepository : IBaseRepository<Entities.Models.Department>
{
    Task<ICollection<DepartmentDto>> GetAllDepartments(DepartmentFilter filter);
    Task<bool> ExistsAsync(string title);
    Task<bool> ExistsEmployees(int departmentId);
}