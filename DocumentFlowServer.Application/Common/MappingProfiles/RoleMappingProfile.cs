using AutoMapper;
using DocumentFlowServer.Application.Role.Dtos;

namespace DocumentFlowServer.Application.Common.MappingProfiles;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        CreateMap<Entities.Models.AboutUserModels.Role, RoleDto>();
    }
}