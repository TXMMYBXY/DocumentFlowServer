using AutoMapper;
using DocumentFlowServer.Application.Repository.Role;
using DocumentFlowServer.Application.Services.Role;
using DocumentFlowServer.Application.Services.Role.Dto;

namespace DocumentFlowServer.Infrastructure.Services;

public class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly IRoleRepository _roleRepository;

    public RoleService(IMapper mapper, IRoleRepository roleRepository)
    {
        _mapper = mapper;
        _roleRepository = roleRepository;
    }
    
    public async Task<List<GetRoleDto>> GetAllRolesAsync()
    {
        var listRole = await _roleRepository.GetRolesAsync();
        var listRoleDto = _mapper.Map<List<GetRoleDto>>(listRole);
        
        return listRoleDto;
    }
}