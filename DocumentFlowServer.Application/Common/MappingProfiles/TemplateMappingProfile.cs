using AutoMapper;
using DocumentFlowServer.Application.Template.Dtos;
using DocumentFlowServer.Entities.Models.DocumentTemplatesModels;

namespace DocumentFlowServer.Application.Common.MappingProfiles;

public class TemplateMappingProfile : Profile
{
    public TemplateMappingProfile()
    {
        CreateMap<StatementTemplate, GetTemplateForWorkerDto>();
    }
}