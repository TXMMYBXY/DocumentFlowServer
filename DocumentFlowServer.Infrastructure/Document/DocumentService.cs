using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DocumentFlowServer.Application.Common.Services;
using DocumentFlowServer.Application.Document;
using DocumentFlowServer.Application.Document.Dtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace DocumentFlowServer.Infrastructure.Document;

public class DocumentService : IDocumentService
{
    private const string DocumentsVersionKey = "documents_version";
    
    private readonly ILogger<DocumentService> _logger;
    private readonly IDistributedCache _cache;
    private readonly IHubContext<DocumentHub> _documentHub;
    private readonly IFileStorageService _fileStorageService;
    
    private readonly IDocumentRepository _documentRepository;

    public DocumentService(
        ILogger<DocumentService> logger,
        IDistributedCache cache,
        IHubContext<DocumentHub> documentHub,
        IFileStorageService fileStorageService,
        IDocumentRepository documentRepository)
    {
        _logger = logger;
        _cache = cache;
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
            Type = documentDto.DocumentType
        };

        await _documentRepository.AddAsync(documentModel);
        await _documentRepository.SaveChangesAsync();
        
        await _documentHub.Clients.User(documentDto.CreatedBy.ToString())
            .SendAsync("downloadDocument", documentModel.Id);

        await _InvalidateDocumentsCacheAsync();
        
        _logger.LogInformation("Document created successfully with title {Title}", documentDto.Title);
    }

    public async Task<PagedDocumentDto> GetAllDocumentsByUserId(int userId, DocumentFilter filter)
    {
        var version = await _GetDocumentsVersionAsync();
        var serializedFilter = JsonSerializer.Serialize(filter);
        var cacheKey = $"documents_{version}_{serializedFilter}";

        var cached = await _cache.GetStringAsync(cacheKey);

        if (cached != null)
        {
            return JsonSerializer.Deserialize<PagedDocumentDto>(cached);
        }

        var documents = await _documentRepository.GetAllDocumentsAsync(userId, filter);
        var totalCount = await _documentRepository.GetCountByUserIdAsync(userId);
        
        var pagedDocumentDto = new PagedDocumentDto
        {
            Documents = documents,
            TotalCount = totalCount,
            PageSize = filter.PageSize ?? totalCount,
            CurrentPage = filter.PageNumber ?? 1
        };
        
        var serializedResult = JsonSerializer.Serialize(pagedDocumentDto);

        await _cache.SetStringAsync(cacheKey, serializedResult, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        });

        return pagedDocumentDto;
    }

    public async Task DeleteDocumentByIdAsync(int documentId)
    {
        await _documentRepository.DeleteAsync(documentId);

        await _InvalidateDocumentsCacheAsync();
    }

    private static string _ClearName(string input)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            input = input.Replace(c, '_');

        return input.Replace(" ", "_");
    }
    
    private async Task<string> _GetDocumentsVersionAsync()
    {
        var version = await _cache.GetStringAsync(DocumentsVersionKey);

        if (version == null)
        {
            version = Guid.NewGuid().ToString();

            await _cache.SetStringAsync(DocumentsVersionKey, version);
        }

        return version;
    }

    private async Task _InvalidateDocumentsCacheAsync()
    {
        await _cache.SetStringAsync(DocumentsVersionKey, Guid.NewGuid().ToString());
    }
}