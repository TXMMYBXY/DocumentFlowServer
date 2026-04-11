using AutoMapper;
using DocumentFlowServer.Api.Controllers.Authorization.ViewModels;
using DocumentFlowServer.Application.Services.Authorization.Dto;
using DocumentFlowServer.Entities.Models;
using DocumentFlowServer.Entities.Models.AboutUserModels;

namespace DocumentFlowServer.Api.Controllers.Authorization;

public class AuthorizationMappingProfile : Profile
{
    public AuthorizationMappingProfile()
    {
        //Profiles for login

        CreateMap<LoginUserDto, LoginRequestViewModel>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
            .ReverseMap();

        CreateMap<LoginResponseDto, LoginResponseViewModel>()
            .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.AccessToken))
            .ForMember(dest => dest.ExpiresAt, opt => opt.MapFrom(src => src.ExpiresAt))
            .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserInfo))
            .ForMember(dest => dest.TokenType, opt => opt.MapFrom(src => src.TokenType))
            .ReverseMap();

        CreateMap<User, UserInfoForLoginDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.Title));


        CreateMap<RefreshTokenToLoginDto, RefreshTokenToLoginViewModel>()
            .ReverseMap();

        CreateMap<RefreshTokenToLoginResponseViewModel, RefreshTokenToLoginResponseDto>()
            .ReverseMap();

        // CreateMap<NewAuthRecordDto, LoginHistory>();

        //profiles for refresh

        CreateMap<RefreshTokenRequestDto, RefreshTokenRequestViewModel>()
            .ReverseMap();

        CreateMap<RefreshTokenResponseViewModel, RefreshTokenResponseDto>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<RefreshToken, RefreshTokenRequestDto>()
            .ReverseMap();

        CreateMap<RefreshTokenResponseDto, RefreshToken>()
            .ReverseMap();

        CreateMap<RefreshToken, RefreshTokenDto>()
            .ReverseMap();

        CreateMap<RefreshTokenDto, RefreshTokenResponseDto>()
            .ReverseMap();

        //profiles for access

        CreateMap<CreateAccessTokenDto, CreateAccessTokenViewModel>()
            .ReverseMap();

        CreateMap<CreateAccessTokenResponseDto, AccessTokenResponseViewModel>()
            .ReverseMap();
    }
}
