using AutoMapper;
using DocumentFlowServer.Api.Controllers.Authorization;
using DocumentFlowServer.Api.Controllers.Role.ViewModels;
using DocumentFlowServer.Application.Services.Role;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowServer.Api.Controllers.Role;

[ApiController]
[Route("api/role")]
[AuthorizeByRoleId]
public class RoleController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRoleService _roleService;

    public RoleController(IMapper mapper, IRoleService roleService)
    {
        _mapper = mapper;
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<ActionResult<List<GetRoleViewModel>>> GetRoles()
    {
        var listRoleDto = await _roleService.GetAllRolesAsync();
        var listRoleViewModel = _mapper.Map<List<GetRoleViewModel>>(listRoleDto);

        return Ok(listRoleViewModel);
    }
}