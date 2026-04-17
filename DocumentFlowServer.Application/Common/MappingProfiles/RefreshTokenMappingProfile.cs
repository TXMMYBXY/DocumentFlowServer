using AutoMapper;
using DocumentFlowServer.Application.RefreshToken.Dtos;

namespace DocumentFlowServer.Application.Common.MappingProfiles;

public class RefreshTokenMappingProfile : Profile
{
    public RefreshTokenMappingProfile()
    {
        CreateMap<Entities.Models.AboutUserModels.RefreshToken, RefreshTokenDto>()
            .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.Token));
    }
}