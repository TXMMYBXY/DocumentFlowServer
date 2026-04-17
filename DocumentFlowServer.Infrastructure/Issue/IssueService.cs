using System.Text.Json;
using AutoMapper;
using DocumentFlowServer.Application.Issue;
using DocumentFlowServer.Application.Issue.Dtos;
using DocumentFlowServer.Entities.Models;
using Microsoft.Extensions.Logging;

namespace DocumentFlowServer.Infrastructure.Issue;

public class IssueService : IIssueService
{
    private readonly ILogger<IssueService> _logger;
    private readonly IMapper _mapper;
    
    private readonly IIssueRepository _issueRepository;

    public IssueService(
        ILogger<IssueService> logger,
        IMapper mapper,
        IIssueRepository issueRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _issueRepository = issueRepository;
    }
    
    public async Task<IssueResultDto> CreateIssueAsync(CreateIssueRequestDto dto)
    {
        _logger.LogInformation("User with id: {UserId} is creating a task of template type: {TemplateType}", dto.UserId, dto.TemplateType);
        
        var templateDataJson = JsonSerializer.Serialize(dto.Data);

        var issue = _mapper.Map<IssueModel>(dto);

        issue.TemplateData = templateDataJson;

        await _issueRepository.AddAsync(issue);
        await _issueRepository.SaveChangesAsync();

        _logger.LogInformation("User with id: {UserId} successfully created a task with id: {TaskId} of type: {TemplateType}",
            dto.UserId, issue.TaskId, dto.TemplateType);

        return new IssueResultDto
        {
            TaskId = issue.TaskId,
            Status = issue.Status,
            Message = "Задача успешно создана"
        };
    }
}