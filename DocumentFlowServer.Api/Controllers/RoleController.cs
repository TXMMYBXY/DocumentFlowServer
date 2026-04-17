using AutoMapper;
using DocumentFlowServer.Api.Authorization.Policies;
using DocumentFlowServer.Api.Features.Role;
using DocumentFlowServer.Application.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowServer.Api.Controllers;

[ApiController]
[Route("api/role")]
[Authorize(Policy = Policy.All)]
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
    public async Task<ActionResult<List<RoleResponse>>> GetRoles()
    {
        var listRoleDto = await _roleService.GetRolesAsync();
        var listRoleViewModel = _mapper.Map<List<RoleResponse>>(listRoleDto);

        return Ok(listRoleViewModel);
    }
}