using AutoMapper;
using DocumentFlowServer.Api.Controllers.Role.ViewModels;
using DocumentFlowServer.Application.Services.Role.Dto;

namespace DocumentFlowServer.Api.Controllers.Role.MappingProfile;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        //Profiles for GET
        CreateMap<GetRoleDto, GetRoleViewModel>();

        CreateMap<Entities.Models.AboutUserModels.Role, GetRoleDto>();
        
        CreateMap<Entities.Models.AboutUserModels.Role, RoleDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
    }
}