using AutoMapper;
using DocumentFlowServer.Api.Features.Document.Requests;
using DocumentFlowServer.Api.Features.Document.Responses;
using DocumentFlowServer.Application.Document;
using DocumentFlowServer.Application.Document.Dtos;

namespace DocumentFlowServer.Api.Features.Document;

public class DocumentMappingProfile : Profile
{
    public DocumentMappingProfile()
    {
        CreateMap<DownloadDocumentDto, DownloadDocumentResponse>();

        CreateMap<UploadDocumentRequest, UploadDocumentDto>()
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.File.FileName))
            .ForMember(dest => dest.FileLength, opt => opt.MapFrom(src => src.File.Length))
            .ForMember(dest => dest.FileStream, opt => opt.MapFrom(src => src.File.OpenReadStream()));

        CreateMap<GetDocumentsRequest, DocumentFilter>();
        
        CreateMap<PagedDocumentDto, PagedDocumentResponse>();
    }
}