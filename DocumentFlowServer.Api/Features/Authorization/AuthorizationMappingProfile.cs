using AutoMapper;
using DocumentFlowServer.Api.Features.Authorization.Requests;
using DocumentFlowServer.Api.Features.Authorization.Responses;
using DocumentFlowServer.Application.Account.RequestDto;
using DocumentFlowServer.Application.Account.ResponseDto;
using DocumentFlowServer.Application.Jwt.Dtos;
using DocumentFlowServer.Application.RefreshToken.Dtos;

namespace DocumentFlowServer.Api.Features.Authorization;

public class AuthorizationMappingProfile : Profile
{
    public AuthorizationMappingProfile()
    {
        CreateMap<LoginRequest, LoginRequestDto>();
        
        CreateMap<LoginResponseDto, LoginResponse>()
            .ForMember(dest => dest.Access, opt => opt.MapFrom(src => src.AccessToken))
            .ForMember(dest => dest.Refresh, opt => opt.MapFrom(src => src.RefreshToken));

        CreateMap<LoginRefreshResponseDto, LoginRefreshResponse>();

        CreateMap<AccessTokenDto, AccessTokenResponse>();

        CreateMap<RefreshTokenDto, RefreshTokenResponse>();
    }
}