using System.Security.Claims;
using System.Security.Cryptography;
using AutoMapper;
using DocumentFlowServer.Api.Authorization.Policies;
using DocumentFlowServer.Api.Features.Document.Requests;
using DocumentFlowServer.Api.Features.Document.Responses;
using DocumentFlowServer.Api.Features.Templates;
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

    [Authorize(Policy = Policy.All)]
    [HttpGet]
    public async Task<ActionResult<PagedDocumentResponse>> GetDocuments([FromQuery] GetDocumentsRequest request)
    {
        _logger.LogInformation("Request returns users");
        
        var filter = _mapper.Map<GetDocumentsRequest, DocumentFilter>(request);
        
        var result = await _documentService.GetAllDocumentsByUserId(
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value), filter);
        
        var response = _mapper.Map<PagedDocumentResponse>(result);

        return Ok(response);
    }

    [HttpDelete("{documentIdL:int}")]
    [Authorize(Policy = Policy.All)]
    public async Task<ActionResult> DeleteDocument([FromRoute] int documentId)
    {
        _logger.LogInformation("request for deleting document");

        await _documentService.DeleteDocumentByIdAsync(documentId);
        
        return Ok();
    }
    
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [WorkerAuthorize]
    public async Task<ActionResult> UploadDocument([FromForm] UploadDocumentRequest request)
    {
        _logger.LogInformation("request for uploading document");

        var documentDto = _mapper.Map<UploadDocumentDto>(request);
        
        await _documentService.UploadDocumentAsync(documentDto);

        return Ok();
    }
}