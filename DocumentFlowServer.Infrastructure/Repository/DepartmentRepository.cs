using DocumentFlowServer.Application.Repository.Department;
using DocumentFlowServer.Application.Repository.Department.Dto;
using DocumentFlowServer.Application.Repository.User.Dto;
using DocumentFlowServer.Application.Services.Department;
using DocumentFlowServer.Entities.Data;
using DocumentFlowServer.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Repository;

public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public DepartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<DepartmentDto>> GetAllDepartmentsAsync(DepartmentFilter filter)
    {
        var query = _dbContext.Departments
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.Title))
        {
            query = query.Where(d => d.Title.Contains(filter.Title));
        }

        var resultQuery = query.Select(d => new DepartmentDto
        {
            Id = d.Id,
            Title = d.Title,
            Description = d.Description,
            Employees = d.Employees
                .Where(e => e.DepartmentId == d.Id)
                .Select(e => new UserInfoDto
                {
                    Id = e.Id,
                    Email = e.Email,
                    FullName = e.FullName,
                    Department = e.Department.Title,
                    Role = e.Role.Title
                })
                .ToList()
        });

        if (filter.PageSize.HasValue && filter.PageNumber.HasValue)
        {
            resultQuery = resultQuery
                .Skip((filter.PageNumber.Value - 1) * filter.PageSize.Value)
                .Take(filter.PageSize.Value);
        }

        return await resultQuery
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> IsDepartmentHasEmployeesAsync(int departmentId)
    {
        return await _dbContext.Departments.Where(d => d.Id == departmentId)
            .Select(d => d.Employees.Any())
            .FirstOrDefaultAsync();
    }
}