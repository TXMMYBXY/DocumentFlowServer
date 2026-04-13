using AutoMapper;
using DocumentFlowServer.Api.Features.Departments.Requests;
using DocumentFlowServer.Api.Features.Departments.Responses;
using DocumentFlowServer.Application.Department;
using DocumentFlowServer.Application.Department.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowServer.Api.Controllers;

[ApiController]
[Route("api/department")]
[Authorize]
public class DepartmentController : ControllerBase
{
    private readonly ILogger<DepartmentController> _logger;
    private readonly IMapper _mapper;
    private readonly IDepartmentService _departmentService;

    public DepartmentController(
        ILogger<DepartmentController> logger, 
        IMapper mapper,
        IDepartmentService departmentService)
    {
        _logger = logger;
        _mapper = mapper;
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedDepartmentResponse>> GetAllDepartments([FromQuery] GetDepartmentsRequest request)
    {
        _logger.LogInformation("Requesting returns departments");
        
        var filter = _mapper.Map<DepartmentFilter>(request);
        
        var departments = await _departmentService.GetDepartmentAsync(filter);
        
        var response =  _mapper.Map<PagedDepartmentResponse>(departments);
        
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> CreateDepartment(CreateDepartmentRequest request)
    {
        _logger.LogInformation("Requesting create department");
        
        var departmentDto = _mapper.Map<CreateDepartmentDto>(request);
        
        await _departmentService.CreateDepartment(departmentDto);
        
        return Ok();
    }

    [HttpPatch("{departmentId:int}")]
    public async Task<ActionResult> UpdateDepartment([FromRoute] int departmentId, UpdateDepartmentRequest request)
    {
        _logger.LogInformation("Requesting update department");

        var departmentDto = _mapper.Map<UpdateDepartmentDto>(request);
        
        await _departmentService.UpdateDepartment(departmentId, departmentDto);
        
        return Ok();
    }

    [HttpDelete("{departmentId:int}")]
    public async Task<ActionResult> DeleteDepartment([FromRoute] int departmentId)
    {
        _logger.LogInformation("Requesting deletes department");
        
        await _departmentService.DeleteDepartmentAsync(departmentId);
        
        return Ok();
    }
}