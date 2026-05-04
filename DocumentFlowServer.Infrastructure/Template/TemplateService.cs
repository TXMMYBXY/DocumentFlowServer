using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using DocumentFlowServer.Application.Common.Services;
using DocumentFlowServer.Application.FieldExtractor;
using DocumentFlowServer.Application.FieldExtractor.Dtos;
using DocumentFlowServer.Application.Template;
using DocumentFlowServer.Application.Template.Dtos;
using DocumentFlowServer.Entities.Enums;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace DocumentFlowServer.Infrastructure.Template;

public class TemplateService : ITemplateService
{
    private readonly string _templatesVersionKey = $"template_version";
    private const string FieldsVersionKey = "fields_version";
    
    private readonly ILogger<TemplateService> _logger;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly IFileStorageService _fileStorageService;
    private readonly IFieldExtractorService _fieldExtractor;
    private readonly INotificationService _notificationService;
    
    private readonly ITemplateRepository _templateRepository;

    public TemplateService(
        ILogger<TemplateService> logger,
        IMapper mapper,
        IDistributedCache cache,
        IFileStorageService fileStorageService,
        IFieldExtractorService fieldExtractor,
        INotificationService notificationService,
        ITemplateRepository templateRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _cache = cache;
        _fileStorageService = fileStorageService;
        _fieldExtractor = fieldExtractor;
        _notificationService = notificationService;
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
        
        var templatesTuple = await _templateRepository.GetAllTemplatesAsync(filter);

        var pagedTemplateDto = new PagedTemplateDto
        {
            Templates = templatesTuple.Item1,
            TotalCount = templatesTuple.Item2,
            PageSize = filter.PageSize ?? templatesTuple.Item2,
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
        var month = _ClearName(DateTime.Now.ToString("MMMM", new CultureInfo("en-EN")));
        var projectFolder = $"{templateDto.Type.ToString()}_{DateTime.Now.Year}_{month}";

        var filePath = await _fileStorageService.SaveFileAsync(
            templateDto.FileStream,
            uniqueFileName,
            projectFolder);

        var templateModel = new Entities.Models.Template
        {
            Title = templateDto.Title,
            Path = filePath,
            CreatedBy = templateDto.CreatedBy,
            CreatedAt = DateTime.UtcNow,
            IsActive = templateDto.IsActive,
            Type = templateDto.Type
        };

        await _templateRepository.AddAsync(templateModel);
        await _templateRepository.SaveChangesAsync();
        
        await _notificationService.SendNotificationToRoleAsync(templateDto.Type == TemplateType.Contract?
            [1, 2, 3] : [1, 2, 3, 4], new Entities.Models.Notification(
            NotificationKind.TemplateAdded,
            NotificationSeverity.Info,
            "Шаблон добавлен",
            $"Добавлен новый шаблон {templateModel.Title}"
        ));
        
        await _InvalidateTemplatesCacheAsync();
        
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

        if (template.Type == TemplateType.Contract)
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
        
        var type = await _templateRepository.GetTypeByTemplateIdAsync(templateId);
        
        string filePath = null;
        
        if (templateDto.FileStream != null && templateDto.FileLength != 0)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{templateDto.FileName}";
            var month = _ClearName(DateTime.Now.ToString("MMMM", new CultureInfo("ru-RU")));
            var projectFolder = $"{type}_{DateTime.Now.Year}_{month}";
        
            var oldFilePath = await _templateRepository.GetFilePathAsync(templateId);
        
            await _fileStorageService.DeleteFileAsync(oldFilePath);
        
            filePath = await _fileStorageService.SaveFileAsync(
                templateDto.FileStream,
                uniqueFileName,
                projectFolder);
        }
        
        await _templateRepository.UpdateTemplatePartialAsync(templateId, templateDto.Title, filePath);
        await _templateRepository.SaveChangesAsync();
        
        await _notificationService.SendNotificationToRoleAsync(type == TemplateType.Contract?
            [1, 2, 3] : [1, 2, 3, 4], new Entities.Models.Notification(
            NotificationKind.TemplateAdded,
            NotificationSeverity.Info,
            "Шаблон обновлен",
            $"Добавлен новый шаблон {templateDto.Title}"
        ));
        
        await _InvalidateTemplatesCacheAsync();
        
        _logger.LogInformation("Template updated successfully with id {TemplateId}", templateId);
    }

    public async Task DeleteTemplateAsync(int templateId)
    {
        _logger.LogInformation("Deleting template with id {TemplateId}", templateId);
        
        var type = await _templateRepository.GetTypeByTemplateIdAsync(templateId);
        
        await _templateRepository.DeleteAsync(templateId);
        await _templateRepository.SaveChangesAsync();

        await _InvalidateTemplatesCacheAsync();
        
        await _notificationService.SendNotificationToRoleAsync(type == TemplateType.Contract?
            [1, 2, 3] : [1, 2, 3, 4], new Entities.Models.Notification(
            NotificationKind.TemplateDeleted,
            NotificationSeverity.Info,
            "Шаблон обновлен",
            $"Удален шаблон под номером {templateId}"
        ));
        
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
        var template = await _templateRepository.GetTemplateForDownloadingByIdAsync(templateId);

        ArgumentNullException.ThrowIfNull(template, "template is not exist");

        return new DownloadTemplateDto
        {
            FilePath = template.Path,
            FileName = template.Title
        };
    }

    public async Task DeleteManyTemplatesAsync(List<int> templateIds)
    {
        await _templateRepository.DeleteManyAsync(templateIds);
        await _templateRepository.SaveChangesAsync();
        
        await _InvalidateTemplatesCacheAsync();
    }

    public async Task<GetTemplateForWorkerDto> GetTemplateForWorkerByIdAsync(int templateId)
    {
        var template = await _templateRepository.GetByIdAsync(templateId);

        return _mapper.Map<GetTemplateForWorkerDto>(template);
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