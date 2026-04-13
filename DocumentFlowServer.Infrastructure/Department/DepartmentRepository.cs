using DocumentFlowServer.Application.Department;
using DocumentFlowServer.Application.Department.Dtos;
using DocumentFlowServer.Application.Role.Dtos;
using DocumentFlowServer.Application.User.Dtos;
using DocumentFlowServer.Infrastructure.Common.Repository;
using DocumentFlowServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Department;

public class DepartmentRepository : BaseRepository<Entities.Models.Department>, IDepartmentRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public DepartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ICollection<DepartmentDto>> GetAllDepartments(DepartmentFilter filter)
    {
        var query = _dbContext.Departments
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.Title)) query = query.Where(d => d.Title.Contains(filter.Title));
        if (filter.PageSize.HasValue && filter.PageNumber.HasValue)
        {
            query = query
                .Skip((filter.PageNumber.Value - 1) * filter.PageSize.Value)
                .Take(filter.PageSize.Value);
        }
        
        var resultQuery = await query
            .Select(d => new DepartmentDto
            {
                Id  = d.Id,
                Title = d.Title,
                Description = d.Description,
                Employees = d.Employees.Select(u => new UserCleanDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    FullName = u.FullName,
                    IsActive = u.IsActive,
                    Role = new RoleDto
                    {
                        Id = u.Role.Id,
                        Title = u.Role.Title,
                        Description = u.Role.Description
                    }
                }).ToList()
            })
            .ToListAsync();
        
        return resultQuery;
    }

    public async Task<bool> ExistsAsync(string title)
    {
        return await _dbContext.Departments
            .AnyAsync(d => d.Title == title ||  d.Title == title.ToLower());
    }

    public async Task<bool> ExistsEmployees(int departmentId)
    {
        return await _dbContext.Departments
            .Where(d => d.Id == departmentId)
            .Select(d => d.Employees.Count == 0)
            .SingleAsync();
    }
}