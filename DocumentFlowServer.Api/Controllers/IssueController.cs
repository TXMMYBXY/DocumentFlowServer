using System.Security.Claims;
using AutoMapper;
using DocumentFlowServer.Api.Authorization.Policies;
using DocumentFlowServer.Api.Features.Issue.Requests;
using DocumentFlowServer.Api.Features.Issue.Responses;
using DocumentFlowServer.Application.Issue;
using DocumentFlowServer.Application.Issue.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowServer.Api.Controllers;

[ApiController]
[Route("api/issue")]
[Authorize(Policy = Policy.All)]
public class IssueController : ControllerBase
{
    private readonly ILogger<IssueController> _logger;
    private readonly IMapper _mapper;
    private readonly IIssueService _issueService;
    
    public IssueController(
        ILogger<IssueController> logger,
        IMapper mapper,
        IIssueService issueService)
    {
        _logger = logger;
        _mapper = mapper;
        _issueService = issueService;
    }

    [HttpPost("generate")]
    public async Task<ActionResult> CreateIssue([FromBody] CreateIssueRequest request)
    {
        var taskDtoRequest = _mapper.Map<CreateIssueRequestDto>(request);
        
        taskDtoRequest.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var result = await _issueService.CreateIssueAsync(taskDtoRequest);
        
        var response = _mapper.Map<CreateIssueResponse>(result);

        return Accepted(response);
    }
}