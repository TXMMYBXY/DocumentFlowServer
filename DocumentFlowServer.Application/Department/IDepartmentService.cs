using System.Threading.Tasks;
using DocumentFlowServer.Application.Department.Dtos;

namespace DocumentFlowServer.Application.Department;

public interface IDepartmentService
{
    Task<PagedDepartmentDto> GetDepartmentAsync(DepartmentFilter filter);
    Task CreateDepartment(CreateDepartmentDto dto);
    Task UpdateDepartment(int departmentId, UpdateDepartmentDto dto);
    Task DeleteDepartment(int departmentId);
}