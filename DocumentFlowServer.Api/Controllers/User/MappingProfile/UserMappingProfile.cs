using AutoMapper;
using DocumentFlowServer.Api.Controllers.User.ViewModels;
using DocumentFlowServer.Application.Repository.User.Dto;
using DocumentFlowServer.Application.Services.User.Dto;

namespace DocumentFlowServer.Api.Controllers.User.MappingProfile;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        //Profiles for GET

        CreateMap<UserEntity, GetUserDto>()
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.Title))
            .ReverseMap();

        CreateMap<GetUserDto, GetUserViewModel>().ReverseMap();

        CreateMap<PagedUserDto, PagedUserViewModel>()
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users))
            .ReverseMap();

        CreateMap<Entities.Models.AboutUserModels.User, GetUserDto>()
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.Title))
            .ReverseMap();

        //Profiles for POST

        CreateMap<CreateUserViewModel, CreateUserDto>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
            .ReverseMap();

        CreateMap<CreateUserDto, Entities.Models.AboutUserModels.User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        //Profiles for PATCH

        CreateMap<UpdateUserViewModel, UpdateUserDto>().ReverseMap();

        CreateMap<UpdateUserDto, Entities.Models.AboutUserModels.User>()
            .ForMember(dest => dest.FullName, opt =>
            {
                opt.PreCondition(src => src.FullName != null);
                opt.MapFrom(src => src.FullName);
            })
            .ForMember(dest => dest.Email, opt =>
            {
                opt.PreCondition(src => src.Email != null);
                opt.MapFrom(src => src.Email);
            })
            .ForMember(dest => dest.DepartmentId, opt =>
            {
                opt.PreCondition(src => src.DepartmentId.HasValue);
                opt.MapFrom(src => src.DepartmentId!.Value);
            })
            .ForMember(dest => dest.RoleId, opt =>
            {
                opt.PreCondition(src => src.RoleId.HasValue);
                opt.MapFrom(src => src.RoleId!.Value);
            })
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore());
            
        CreateMap<ResetPasswordViewModel, ResetPasswordDto>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
    }
}
