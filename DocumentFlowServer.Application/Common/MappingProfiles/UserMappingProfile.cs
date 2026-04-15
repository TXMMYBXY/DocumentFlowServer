using AutoMapper;
using DocumentFlowServer.Application.User.Dtos;

namespace DocumentFlowServer.Application.Common.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        //SetNewUserPassword
        CreateMap<CreateUserDto, Entities.Models.AboutUserModels.User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
        
        //PartialUpdateUserInfo
        CreateMap<UpdateUserInfoDto, Entities.Models.AboutUserModels.User>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        
        //CreateNewUser
        CreateMap<CreateUserDto, Entities.Models.AboutUserModels.User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
        
        //RefreshTokenDtos
        CreateMap<UserDto, Entities.Models.AboutUserModels.User>();
        
    }
}