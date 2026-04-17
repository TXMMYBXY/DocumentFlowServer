using AutoMapper;
using DocumentFlowServer.Application.Personal.Dtos;
using DocumentFlowServer.Entities.Models;

namespace DocumentFlowServer.Application.Common.MappingProfiles;

public class LoginTimeMappingProfile : Profile
{
    public LoginTimeMappingProfile()
    {
        CreateMap<AuthRecordDto, LoginHistory>();
    }
}