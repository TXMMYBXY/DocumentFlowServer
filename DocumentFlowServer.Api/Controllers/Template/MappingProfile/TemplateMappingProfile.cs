using AutoMapper;
using DocumentFlowServer.Api.Controllers.Template.ViewModels;
using DocumentFlowServer.Application.Services.Template.Dto;

namespace DocumentFlowServer.Api.Controllers.Template.MappingProfile;

public class TemplateMappingProfile : Profile
{
    public TemplateMappingProfile()
    {
        //Profiles for GET

        CreateMap<GetTemplateViewModel, GetTemplateDto>().ReverseMap();

        CreateMap<Entities.Models.DocumentTemplatesModels.Template, GetTemplateDto>().ReverseMap();

        CreateMap<PagedTemplateViewModel, PagedTemplateDto>().ReverseMap();

        CreateMap<DownloadTemplateDto, DownloadTemplateViewModel>().ReverseMap();

        CreateMap<GetTemplateForWorkerDto, GetTemplateForWorkerViewModel>();

        CreateMap<WorkerTemplateDto, GetTemplateForWorkerDto>();

        //Profiles for CREATE

        CreateMap<CreateTemplateViewModel, CreateTemplateDto>()
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.File.FileName))
            .ForMember(dest => dest.FileLength, opt => opt.MapFrom(src => src.File.Length))
            .ForMember(dest => dest.FileStream, opt => opt.MapFrom(src => src.File.OpenReadStream()));

        //Profiles for UPDATE

        CreateMap<UpdateTemplateViewModel, UpdateTemplateDto>()
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.File.FileName))
            .ForMember(dest => dest.FileLength, opt => opt.MapFrom(src => src.File.Length))
            .ForMember(dest => dest.FileStream, opt => opt.MapFrom(src => src.File.OpenReadStream()))
            .ReverseMap();

        //Profiles for Extract

        CreateMap<TemplateFieldInfoDto, TemplateFieldInfoViewModel>().ReverseMap();
    }
}
