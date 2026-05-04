using System.Globalization;
using DocumentFlowServer.Application.Common.Services;
using DocumentFlowServer.Application.Document;
using DocumentFlowServer.Application.Document.Dtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DocumentFlowServer.Infrastructure.Document;

public class DocumentService : IDocumentService
{
    private readonly ILogger<DocumentService> _logger;
    private readonly IHubContext<DocumentHub> _documentHub;
    private readonly IFileStorageService _fileStorageService;
    
    private readonly IDocumentRepository _documentRepository;

    public DocumentService(
        ILogger<DocumentService> logger,
        IHubContext<DocumentHub> documentHub,
        IFileStorageService fileStorageService,
        IDocumentRepository documentRepository)
    {
        _logger = logger;
        _documentHub = documentHub;
        _fileStorageService = fileStorageService;
        _documentRepository = documentRepository;
    }
    
    public async Task<DownloadDocumentDto> DownloadDocumentAsync(int documentId)
    {
        var documentDto = await _documentRepository.GetFilledDocumentByIdAsync(documentId);

        return new DownloadDocumentDto
        {
            FilePath = documentDto.FilePath,
            FileName = documentDto.Title
        };
    }

    public async Task UploadDocumentAsync(UploadDocumentDto documentDto)
    {
        _logger.LogInformation("Creating new template with title {Title}", documentDto.Title);
        
        ArgumentNullException.ThrowIfNull(documentDto, "File is not exists");
        
        if (documentDto.FileLength == 0)
        {
            throw new ArgumentException("File is empty");
        }
        
        var uniqueFileName = $"{Guid.NewGuid()}_{documentDto.FileName}";
        var month = _ClearName(DateTime.Now.ToString("MMMM", new CultureInfo("en-EN")));
        var projectFolder = $"{documentDto.DocumentType}_Owner-{documentDto.CreatedBy}_{DateTime.Now.Year}_{month}";
        
        var filePath = await _fileStorageService.SaveFileAsync(
            documentDto.FileStream,
            uniqueFileName,
            projectFolder);
        
        var documentModel = new Entities.Models.Document
        {
            Title = documentDto.Title,
            Path = filePath,
            CreatedBy = documentDto.CreatedBy,
            TemplateId = documentDto.TemplateId,
            CreatedAt = documentDto.CreatedAt ?? DateTime.UtcNow,
        };

        await _documentRepository.AddAsync(documentModel);
        await _documentRepository.SaveChangesAsync();
        
        await _documentHub.Clients.User(documentDto.CreatedBy.ToString())
            .SendAsync("downloadDocument", documentModel.Id);
        
        
        _logger.LogInformation("Template created successfully with title {Title}", documentDto.Title);
    }
    
    private static string _ClearName(string input)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            input = input.Replace(c, '_');

        return input.Replace(" ", "_");
    }
}