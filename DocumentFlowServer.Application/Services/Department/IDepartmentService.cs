using DocumentFlowServer.Application.Services.Department.Dto;

namespace DocumentFlowServer.Application.Services.Department;

public interface IDepartmentService
{
    Task<PagedDepartmentDto> GetAllDepartmentsAsync(DepartmentFilter filter);
    Task<GetDepartmentDto> GetDepartmentByIdAsync(int id);
    Task CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto);
    Task UpdateDepartmentAsync(int departmetnId, UpdateDepartmentDto updateDepartmentDto);
    Task DeleteDepartmentAsync(int id);
}