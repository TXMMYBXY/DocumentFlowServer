using System.Globalization;
using System.Text;
using System.Text.Json;
using AutoMapper;
using DocumentFlowServer.Application.Common.Services;
using DocumentFlowServer.Application.FieldExtractor;
using DocumentFlowServer.Application.FieldExtractor.Dtos;
using DocumentFlowServer.Application.Template;
using DocumentFlowServer.Application.Template.Dtos;
using DocumentFlowServer.Entities.Models.DocumentTemplatesModels;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace DocumentFlowServer.Infrastructure.Template;

public class TemplateService<T> : ITemplateService<T> where T : Entities.Models.DocumentTemplatesModels.Template, new()
{
    private readonly string _templatesVersionKey = $"{typeof(T).Name}_version";
    private const string FieldsVersionKey = "fields_version";
    
    private readonly ILogger<TemplateService<T>> _logger;
    private readonly IDistributedCache _cache;
    private readonly IFileStorageService _fileStorageService;
    private readonly IFieldExtractorService _fieldExtractor;
    
    private readonly ITemplateRepository<T> _templateRepository;

    public TemplateService(
        ILogger<TemplateService<T>> logger,
        IDistributedCache cache,
        IFileStorageService fileStorageService,
        IFieldExtractorService fieldExtractor,
        ITemplateRepository<T> templateRepository)
    {
        _logger = logger;
        _cache = cache;
        _fileStorageService = fileStorageService;
        _fieldExtractor = fieldExtractor;
        _templateRepository = templateRepository;
    }
    
    public async Task<PagedTemplateDto> GetAllTemplatesAsync(TemplateFilter filter)
    {
        var version = await _GetTemplatesVersionAsync();
        var serializedFilter = JsonSerializer.Serialize(filter);
        var cacheKey = $"templates_{version}_{serializedFilter}";

        var cached = await _cache.GetStringAsync(cacheKey);

        if (cached != null)
        {
            return JsonSerializer.Deserialize<PagedTemplateDto>(cached);
        }
        
        var templates = await _templateRepository.GetAllTemplatesAsync(filter);
        var totalCount = await _templateRepository.GetCountAsync();

        var pagedTemplateDto = new PagedTemplateDto
        {
            Templates = templates,
            TotalCount = totalCount,
            PageSize = filter.PageSize ?? totalCount,
            CurrentPage = filter.PageNumber ?? 1
        };
        
        var serializedResult = JsonSerializer.Serialize(pagedTemplateDto);

        await _cache.SetStringAsync(cacheKey, serializedResult, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        });
        
        return pagedTemplateDto;
    }

    public async Task CreateTemplateAsync(CreateTemplateDto templateDto)
    {
        _logger.LogInformation("Creating new template with title {Title}", templateDto.Title);

        ArgumentNullException.ThrowIfNull(templateDto, "File is not exists");

        if (templateDto.FileLength == 0)
        {
            throw new ArgumentException("File is empty");
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{templateDto.FileName}";
        var month = _ClearName(DateTime.Now.ToString("MMMM", new CultureInfo("ru-RU")));
        var projectFolder = $"{typeof(T).Name}_{DateTime.Now.Year}_{month}";

        var filePath = await _fileStorageService.SaveFileAsync(
            templateDto.FileStream,
            uniqueFileName,
            projectFolder);

        var templateModel = new T
        {
            Title = templateDto.Title,
            Path = filePath,
            CreatedBy = templateDto.CreatedBy,
            CreatedAt = templateDto.CreatedAt,
            IsActive = templateDto.IsActive
        };

        await _templateRepository.AddAsync(templateModel);
        await _templateRepository.SaveChangesAsync();

        await _InvalidateTemplatesCacheAsync();
        
        _logger.LogDebug("Sending notification");
        
        _logger.LogInformation("Template created successfully with title {Title}", templateDto.Title);
    }
    
    public async Task<List<TemplateFieldInfoDto>> ExtractFieldsFromTemplateAsync(int templateId)
    {
        _logger.LogInformation("Extracting fields from template with id {TemplateId}", templateId);

        var version = await _GetFieldsVersionAsync();
        var targetTemplate = JsonSerializer.Serialize(templateId);
        var cacheKey = $"fields_{version}_{targetTemplate}";

        var cached = await _cache.GetStringAsync(cacheKey);

        if (cached != null)
        {
            return JsonSerializer.Deserialize<List<TemplateFieldInfoDto>>(cached);
        }

        var template = await _templateRepository.GetByIdAsync(templateId);
        
        ArgumentNullException.ThrowIfNull(template);

        if (typeof(T) == typeof(ContractTemplate))
        {
            var contractText = _ReadDocx(template.Path);

            var response = _ConvertResponse<List<TemplateFieldInfoDto>>(contractText);

            return response;
        }

        var fieldsDto = await _fieldExtractor.ExtractFieldsAsync(template.Path);

        _logger.LogInformation("Fields extracted successfully from template with id {TemplateId}. Extracted fields count: {FieldsCount}",
            targetTemplate, fieldsDto.Count);

        var serializedResult = JsonSerializer.Serialize(fieldsDto);

        await _cache.SetStringAsync(cacheKey, serializedResult, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        });

        return fieldsDto;
    }

    public async Task UpdateTemplatePartialAsync(int templateId, UpdateTemplateDto templateDto)
    {
        _logger.LogInformation("Updating template with id {TemplateId}", templateId);
        
        string filePath = null;
        
        if (templateDto.FileStream != null && templateDto.FileLength != 0)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{templateDto.FileName}";
            var month = _ClearName(DateTime.Now.ToString("MMMM", new CultureInfo("ru-RU")));
            var projectFolder = $"{typeof(T).Name}_{DateTime.Now.Year}_{month}";
        
            var oldFilePath = await _templateRepository.GetFilePathAsync(templateId);
        
            await _fileStorageService.DeleteFileAsync(oldFilePath);
        
            filePath = await _fileStorageService.SaveFileAsync(
                templateDto.FileStream,
                uniqueFileName,
                projectFolder);
        }
        
        await _templateRepository.UpdateTemplatePartialAsync(templateId, templateDto.Title, filePath);
        await _templateRepository.SaveChangesAsync();

        await _InvalidateTemplatesCacheAsync();
        
        _logger.LogInformation("Template updated successfully with id {TemplateId}", templateId);
    }

    public async Task DeleteTemplateAsync(int templateId)
    {
        _logger.LogInformation("Deleting template with id {TemplateId}", templateId);

        await _templateRepository.DeleteAsync(templateId);
        await _templateRepository.SaveChangesAsync();

        await _InvalidateTemplatesCacheAsync();

        _logger.LogInformation("Template deleted successfully with id {TemplateId}", templateId);
    }

    public async Task<bool> ChangeTemplateStatusById(int templateId)
    {
        _logger.LogInformation("Changing template status for template with id {TemplateId}", templateId);

        var isActive = await _templateRepository.UpdateTemplateStatusAsync(templateId);

        await _templateRepository.SaveChangesAsync();

        _logger.LogInformation("Template status changed successfully for template with id {TemplateId}. New status: {IsActive}",
            templateId, isActive);

        await _InvalidateTemplatesCacheAsync();
        
        _logger.LogDebug("Sending notification");

        return isActive;
    }

    public async Task<DownloadTemplateDto> DownloadTemplateAsync(int templateId)
    {
        var template = await _templateRepository.GetTemplateByIdAsync(templateId);

        ArgumentNullException.ThrowIfNull(template, "template is not exist");

        return new DownloadTemplateDto
        {
            FilePath = template.Path,
            FileName = template.Title
        };
    }

    private async Task<string> _GetTemplatesVersionAsync()
    {
        var version = await _cache.GetStringAsync(_templatesVersionKey);

        if (version == null)
        {
            version = Guid.NewGuid().ToString();

            await _cache.SetStringAsync(_templatesVersionKey, version);
        }

        return version;
    }
    
    private async Task<string> _GetFieldsVersionAsync()
    {
        var version = await _cache.GetStringAsync(FieldsVersionKey);

        if (version == null)
        {
            version = Guid.NewGuid().ToString();

            await _cache.SetStringAsync(FieldsVersionKey, version);
        }

        return version;
    }

    private async Task _InvalidateTemplatesCacheAsync()
    {
        await _cache.SetStringAsync(_templatesVersionKey, Guid.NewGuid().ToString());
    }
    
    private string _ClearName(string input)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            input = input.Replace(c, '_');

        return input.Replace(" ", "_");
    }
    
    private static string _ReadDocx(string filePath)
    {
        StringBuilder sb = new StringBuilder();

        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
        {
            Body body = wordDoc.MainDocumentPart.Document.Body;
            foreach (var para in body.Elements<Paragraph>())
            {
                sb.AppendLine(para.InnerText);
            }
        }

        return sb.ToString();
    }
    
    private static Type? _ConvertResponse<Type>(string response)
    {
        if (!string.IsNullOrWhiteSpace(response))
        {
            return default;
        }

        return JsonSerializer.Deserialize<Type>(response);
    }
}