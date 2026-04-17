using AutoMapper;
using DocumentFlowServer.Application.Role.Dtos;

namespace DocumentFlowServer.Api.Features.Role;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        CreateMap<RoleDto, RoleResponse>();
    }
}