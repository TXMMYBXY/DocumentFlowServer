using AutoMapper;
using DocumentFlowServer.Application.Issue.Dtos;
using DocumentFlowServer.Entities.Enums;
using DocumentFlowServer.Entities.Models;

namespace DocumentFlowServer.Application.Common.MappingProfiles;

public class IssueMappingProfile : Profile
{
    public IssueMappingProfile()
    {
        CreateMap<CreateIssueRequestDto, IssueModel>()
            .ForMember(dest => dest.TaskId, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => IssueStatus.Pending))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}