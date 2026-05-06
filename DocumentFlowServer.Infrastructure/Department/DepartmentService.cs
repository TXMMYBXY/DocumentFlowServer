using System;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using DocumentFlowServer.Application.Common.Services;
using DocumentFlowServer.Application.Department;
using DocumentFlowServer.Application.Department.Dtos;
using DocumentFlowServer.Entities.Enums;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace DocumentFlowServer.Infrastructure.Department;

public class DepartmentService : IDepartmentService
{
    private const string UsersVersionKey = "departments_version";
    
    private ILogger<DepartmentService> _logger;
    private IMapper  _mapper;
    private readonly IDistributedCache _cache;
    private readonly INotificationService _notificationService;
    
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(
        ILogger<DepartmentService> logger,
        IMapper mapper,
        IDistributedCache cache,
        INotificationService notificationService,
        IDepartmentRepository departmentRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _cache = cache;
        _notificationService = notificationService;
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

        await _notificationService.SendNotificationToRoleAsync([1],
            new Entities.Models.Notification(
                NotificationKind.DepartmentAdded,
                NotificationSeverity.Success,
                "Отдел",
                $"Добавлен новый отдел {dto.Title}"));
        
        _logger.LogInformation("Department created successfully");
        
        await _InvalidateDepartmentsCacheAsync();
    }

    public async Task UpdateDepartment(int departmentId, UpdateDepartmentDto dto)
    {
        var department = await _departmentRepository.GetByIdAsync(departmentId);
        
        ArgumentNullException.ThrowIfNull(department);
        
        _mapper.Map(dto, department);

        await _departmentRepository.SaveChangesAsync();
        
        await _notificationService.SendNotificationToRoleAsync([1],
            new Entities.Models.Notification(
                NotificationKind.DepartmentUpdated,
                NotificationSeverity.Success,
                "Отдел",
                $"Изменен отдел {department.Title}"));
        
        _logger.LogInformation("Department updated successfully");
        
        await _InvalidateDepartmentsCacheAsync();
    }

    public async Task DeleteDepartmentAsync(int departmentId)
    {
        var departmentExists = await _departmentRepository.ExistsAsync(departmentId);
        
        if (!departmentExists)
        {
            throw  new NullReferenceException("Department does not exist");
        }
        
        var departmentExistsEmployees = await _departmentRepository.ExistsEmployees(departmentId);

        if (departmentExistsEmployees)
        {
            throw new NullReferenceException("Department has employees");
        }
        
        await _departmentRepository.DeleteAsync(departmentId);
        await _departmentRepository.SaveChangesAsync();
        
        await _notificationService.SendNotificationToRoleAsync([1],
            new Entities.Models.Notification(
                NotificationKind.DepartmentDeleted,
                NotificationSeverity.Success,
                "Отдел",
                $"Добавлен новый отдел под номером {departmentId}"));
        
        _logger.LogInformation("Department deleted successfully");
        
        await _InvalidateDepartmentsCacheAsync();
    }

    public async Task<bool> ExistsDepartmentAsync(int departmentId)
    {
        return await _departmentRepository.ExistsAsync(departmentId);
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