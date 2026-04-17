using AutoMapper;
using DocumentFlowServer.Api.Features.Personal.Requests;
using DocumentFlowServer.Api.Features.Personal.Responses;
using DocumentFlowServer.Application.Personal.Dtos;

namespace DocumentFlowServer.Api.Features.Personal;

public class PersonalAccountMappingProfile : Profile
{
    public PersonalAccountMappingProfile()
    {
        //Changing password
        CreateMap<ChangePasswordRequest, ChangePasswordDto>();
        
        //Getting personal info
        CreateMap<PersonDto, PersonResponse>();
        
        //Getting login times
        CreateMap<LoginTimeDto, LoginTimeResponse>();
    }
}