using System.Security.Claims;
using AutoMapper;
using DocumentFlowServer.Api.Authorization.Policies;
using DocumentFlowServer.Api.Features.Templates.Requests;
using DocumentFlowServer.Api.Features.Templates.Responses;
using DocumentFlowServer.Application.Template;
using DocumentFlowServer.Application.Template.Dtos;
using DocumentFlowServer.Entities.Models.DocumentTemplatesModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowServer.Api.Controllers;

[ApiController]
[Route("api/statement-template")]
[Authorize(Policy = Policy.All)]
public class StatementTemplateController : ControllerBase
{
    private readonly ILogger<StatementTemplateController> _logger;
    private readonly IMapper _mapper;
    private readonly ITemplateService<StatementTemplate> _templateService;

    public StatementTemplateController(
        ILogger<StatementTemplateController> logger,
        IMapper mapper,
        ITemplateService<StatementTemplate> templateService)
    {
        _logger = logger;
        _mapper = mapper;
        _templateService = templateService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedTemplateResponse>> GetAllTemplates([FromQuery] GetTemplatesRequest request)
    {
        _logger.LogInformation("request for getting all templates");
        
        var requestDto = _mapper.Map<TemplateFilter>(request);
        
        var responseDto = await _templateService.GetAllTemplatesAsync(requestDto);
        
        var response = _mapper.Map<PagedTemplateResponse>(responseDto);
        
        return Ok(response);
    }
    
    [HttpGet("{templateId}/extract-fields")]
    public async Task<ActionResult<IReadOnlyList<TemplateFieldInfoResponse>>> ExtractFields([FromRoute] int templateId)
    {
        _logger.LogInformation("request for extracting fields");
        
        var resultDto = await _templateService.ExtractFieldsFromTemplateAsync(templateId);
        var resultViewModel = _mapper.Map<IReadOnlyList<TemplateFieldInfoResponse>>(resultDto);

        return Ok(resultViewModel);
    }
    
    [HttpGet("{templateId:int}/download")]
    public async Task<IActionResult> GetDocument([FromRoute] int templateId)
    {
        _logger.LogInformation("request for downloading template");
        
        var templateDto = await _templateService.DownloadTemplateAsync(templateId);

        var templateViewModel = _mapper.Map<DownloadTemplateResponse>(templateDto);

        var stream = new FileStream(templateViewModel.FilePath, FileMode.Open, FileAccess.Read);

        return File(stream, "application/octet-stream", templateViewModel.FileName);
    }
    
    [Authorize(Policy = Policy.AdminAndBoss)]
    [HttpPost]
    public async Task<ActionResult> CreateTemplate([FromForm] CreateTemplateRequest templateViewModel)
    {
        _logger.LogInformation("request for creating template");
        
        var templateDto = _mapper.Map<CreateTemplateDto>(templateViewModel);

        templateDto.CreatedBy = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        await _templateService.CreateTemplateAsync(templateDto);

        return Ok();
    }
    
    [Authorize(Policy = Policy.AdminAndBoss)]
    [HttpPatch("{templateId}/change-template-status")]
    public async Task<ActionResult<bool>> ChangeTemplateStatus([FromRoute] int templateId)
    {
        _logger.LogInformation("request for change template status");
        
        var status = await _templateService.ChangeTemplateStatusById(templateId);

        return Ok(status);
    }
    
    [HttpPatch("{templateId}/update-template")]
    public async Task<ActionResult> UpdateTemplateByIdPartial([FromRoute] int templateId, 
        [FromForm] UpdateTemplateRequest templateViewModel)
    {
        _logger.LogInformation("request for updating template");
        
        var templateDto = _mapper.Map<UpdateTemplateDto>(templateViewModel);
        
        await _templateService.UpdateTemplatePartialAsync(templateId, templateDto);

        return Ok();
    }
    
    [Authorize(Policy = Policy.AdminAndBoss)]
    [HttpDelete("{templateId}")]
    public async Task<ActionResult> DeleteTemplate([FromRoute] int templateId)
    {
        _logger.LogInformation("request for deleting template");
        
        await _templateService.DeleteTemplateAsync(templateId);

        return Ok();
    }
    
    [Authorize(Policy = Policy.AdminAndBoss)]
    [HttpDelete]
    public async Task<ActionResult> DeleteManyTemplates([FromBody] DeleteManyTemplatesRequest deleteTemplateViewModel)
    {
        _logger.LogInformation("request for deleting many templates");
        
        await _templateService.DeleteManyTemplatesAsync(deleteTemplateViewModel.TemplateIds);

        return Ok();
    }
    
}