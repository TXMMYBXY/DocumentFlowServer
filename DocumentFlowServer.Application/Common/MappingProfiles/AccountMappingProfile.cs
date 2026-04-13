using AutoMapper;
using DocumentFlowServer.Application.Jwt.Dtos;
using DocumentFlowServer.Application.User.Dtos;

namespace DocumentFlowServer.Application.Common.MappingProfiles;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        //Login
        CreateMap<UserLoginDto, UserClaimsDto>();
    }
}