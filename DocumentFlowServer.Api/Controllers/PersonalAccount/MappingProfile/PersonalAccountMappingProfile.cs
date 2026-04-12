using AutoMapper;
using DocumentFlowServer.Api.Controllers.PersonalAccount.ViewModels;
using DocumentFlowServer.Application.Services.Personal.Dto;

namespace DocumentFlowServer.Api.Controllers.PersonalAccount.MappingProfile;

public class PersonalAccountMappingProfile : Profile
{
    public PersonalAccountMappingProfile()
    {
        //Profiles for GET
        CreateMap<GetPersonDto, GetPersonViewModel>();
        
        CreateMap<PersonDto, GetPersonDto>();

        CreateMap<GetLoginTimesDto, GetLoginTimesViewModel>();
        
        CreateMap<LoginTimeDto, GetLoginTimesDto>();
            
        //Profiles for PATCH
        CreateMap<ChangePasswordViewModel, ChangePasswordDto>();
    }
}