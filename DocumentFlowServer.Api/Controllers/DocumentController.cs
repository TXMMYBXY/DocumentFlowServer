using AutoMapper;
using DocumentFlowServer.Api.Authorization.Policies;
using DocumentFlowServer.Api.Features.Document.Requests;
using DocumentFlowServer.Api.Features.Document.Responses;
using DocumentFlowServer.Application.Document;
using DocumentFlowServer.Application.Document.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowServer.Api.Controllers;

[ApiController]
[Route("api/document")]
public class DocumentController : ControllerBase
{
    private readonly ILogger<DocumentController> _logger;
    private readonly IMapper _mapper;
    private readonly IDocumentService _documentService;
    
    public DocumentController(
        ILogger<DocumentController> logger,
        IMapper mapper,
        IDocumentService documentService)
    {
        _logger = logger;
        _mapper = mapper;
        _documentService = documentService;
    }
    
    [Authorize(Policy = Policy.All)]
    [HttpGet("{documentId:int}/download")]
    public async Task<IActionResult> DownloadDocument([FromRoute] int documentId)
    {
        _logger.LogInformation("request for downloading document");
        
        var documentDto = await _documentService.DownloadDocumentAsync(documentId);
        
        var response = _mapper.Map<DownloadDocumentResponse>(documentDto);
        
        var stream = new FileStream(response.FilePath, FileMode.Open, FileAccess.Read);

        return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", response.FileName);
    }
    
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    // [WorkerAuthorize]
    public async Task<ActionResult> UploadDocument([FromForm] UploadDocumentRequest request)
    {
        _logger.LogInformation("request for uploading document");

        var documentDto = _mapper.Map<UploadDocumentDto>(request);
        
        await _documentService.UploadDocumentAsync(documentDto);

        return Ok();
    }
}