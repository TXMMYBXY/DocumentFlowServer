using AutoMapper;
using DocumentFlowServer.Application.User.Dtos;

namespace DocumentFlowServer.Application.Common.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateUserDto, Entities.Models.AboutUserModels.User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
    }
}