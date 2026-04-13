using System.Text.Json;
using AutoMapper;
using DocumentFlowServer.Application.Department;
using DocumentFlowServer.Application.Department.Dtos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace DocumentFlowServer.Infrastructure.Department;

public class DepartmentService : IDepartmentService
{
    private const string UsersVersionKey = "departments_version";
    
    private ILogger<DepartmentService> _logger;
    private IMapper  _mapper;
    private readonly IDistributedCache _cache;
    
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(
        ILogger<DepartmentService> logger,
        IMapper mapper,
        IDistributedCache cache,
        IDepartmentRepository departmentRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _cache = cache;
        _departmentRepository = departmentRepository;
    }
    
    public async Task<PagedDepartmentDto> GetDepartmentAsync(DepartmentFilter filter)
    {
        var version = await _GetDepartmentsVersionAsync();
        var serializedFilter = JsonSerializer.Serialize(filter);
        var cacheKey = $"users_{version}_{serializedFilter}";

        var cached = await _cache.GetStringAsync(cacheKey);

        if (cached != null)
        {
            return JsonSerializer.Deserialize<PagedDepartmentDto>(cached);
        }

        
        var departments = await _departmentRepository.GetAllDepartments(filter);
        var totalCount = await _departmentRepository.GetCountAsync();

        var pagedDepartmentDto = new PagedDepartmentDto
        {
            Departments = departments,
            TotalCount = totalCount,
            PageSize = filter.PageSize ?? totalCount,
            CurrentPage = filter.PageNumber ?? 1
        };
        
        var serializedResult = JsonSerializer.Serialize(pagedDepartmentDto);

        await _cache.SetStringAsync(cacheKey, serializedResult, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
        });
        
        return pagedDepartmentDto;
    }

    public async Task CreateDepartment(CreateDepartmentDto dto)
    {
        _logger.LogInformation("Creating department");

        var departmentExists = await _departmentRepository.ExistsAsync(dto.Title);

        if (departmentExists)
        {
            throw new NullReferenceException("Department already exists");
        }
        
        var department = _mapper.Map<Entities.Models.Department>(dto);
        
        await _departmentRepository.AddAsync(department);
        await _departmentRepository.SaveChangesAsync();
        
        _logger.LogInformation("Department created successfully");
        
        await _InvalidateDepartmentsCacheAsync();
    }

    public async Task UpdateDepartment(int departmentId, UpdateDepartmentDto dto)
    {
        var department = await _departmentRepository.GetByIdAsync(departmentId);
        
        ArgumentNullException.ThrowIfNull(department);
        
        _mapper.Map(dto, department);

        await _departmentRepository.SaveChangesAsync();
        
        _logger.LogInformation("Department updated successfully");
        
        await _InvalidateDepartmentsCacheAsync();
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        var departmentExists = await _departmentRepository.ExistsAsync(id);
        
        if (!departmentExists)
        {
            throw  new NullReferenceException("Department does not exist");
        }
        
        var departmentExistsEmployees = await _departmentRepository.ExistsEmployees(id);

        if (departmentExistsEmployees)
        {
            throw new NullReferenceException("Department has employees");
        }
        
        await _departmentRepository.DeleteAsync(id);
        await _departmentRepository.SaveChangesAsync();
        
        _logger.LogInformation("Department deleted successfully");
        
        await _InvalidateDepartmentsCacheAsync();
    }
    
    private async Task<string> _GetDepartmentsVersionAsync()
    {
        var version = await _cache.GetStringAsync(UsersVersionKey);

        if (version == null)
        {
            version = Guid.NewGuid().ToString();

            await _cache.SetStringAsync(UsersVersionKey, version);
        }

        return version;
    }

    private async Task _InvalidateDepartmentsCacheAsync()
    {
        await _cache.SetStringAsync(UsersVersionKey, Guid.NewGuid().ToString());
    }
}