using AutoMapper;
using DocumentFlowServer.Api.Features.Templates.Requests;
using DocumentFlowServer.Api.Features.Templates.Responses;
using DocumentFlowServer.Application.Template;
using DocumentFlowServer.Application.Template.Dtos;

namespace DocumentFlowServer.Api.Features.Templates;

public class TemplateMappingProfile : Profile
{
    public TemplateMappingProfile()
    {
        //Filter for getting all templates
        CreateMap<GetTemplatesRequest, TemplateFilter>();

        //Mapping paged response from service
        CreateMap<PagedTemplateDto, PagedTemplateResponse>();
        
        //Profile for creating template
        CreateMap<CreateTemplateRequest, CreateTemplateDto>()
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.File.FileName))
            .ForMember(dest => dest.FileLength, opt => opt.MapFrom(src => src.File.Length))
            .ForMember(dest => dest.FileStream, opt => opt.MapFrom(src => src.File.OpenReadStream()));
        
        //Profile for updating template
        CreateMap<UpdateTemplateRequest, UpdateTemplateDto>()
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.File.FileName))
            .ForMember(dest => dest.FileLength, opt => opt.MapFrom(src => src.File.Length))
            .ForMember(dest => dest.FileStream, opt => opt.MapFrom(src => src.File.OpenReadStream()));

    }
}