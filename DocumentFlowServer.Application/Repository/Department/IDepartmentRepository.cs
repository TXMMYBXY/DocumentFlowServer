using DocumentFlowServer.Application.Repository.Department.Dto;
using DocumentFlowServer.Application.Services.Department;

namespace DocumentFlowServer.Application.Repository.Department;

public interface IDepartmentRepository : IBaseRepository<Entities.Models.Department>
{
    Task<List<DepartmentDto>> GetAllDepartmentsAsync(DepartmentFilter filter);
    Task<bool> IsDepartmentHasEmployeesAsync(int departmentId);
    Task<int> GetTotalCountAsync();
}