using AutoMapper;
using DocumentFlowServer.Application.Template.Dtos;

namespace DocumentFlowServer.Application.Common.MappingProfiles;

public class TemplateMappingProfile : Profile
{
    public TemplateMappingProfile()
    {
        CreateMap<Entities.Models.Template, GetTemplateForWorkerDto>();
    }
}