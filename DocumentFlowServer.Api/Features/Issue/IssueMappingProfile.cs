using AutoMapper;
using DocumentFlowServer.Api.Features.Issue.Requests;
using DocumentFlowServer.Api.Features.Issue.Responses;
using DocumentFlowServer.Application.Issue.Dtos;

namespace DocumentFlowServer.Api.Features.Issue;

public class IssueMappingProfile : Profile
{
    public IssueMappingProfile()
    {
        CreateMap<CreateIssueRequest, CreateIssueRequestDto>();
        
        CreateMap<IssueResultDto, CreateIssueResponse>();
    }
}