using System.Net.Http.Headers;
using DocumentFlowServer.Worker.Client.Api.Dto;
using DocumentFlowServer.Worker.Configuration;
using DocumentFlowServer.Worker.Interface.Client;
using DocumentFlowServer.Worker.Models;
using Microsoft.Extensions.Options;

namespace DocumentFlowServer.Worker.Client.Api;

public class ApiClient : GeneralClient, IApiClient
{
    private const string _BaseUrl = "/internal/tasks/worker";
    private readonly string _domain;
    private readonly HttpClient _httpClient;


    public ApiClient(HttpClient httpClient, IOptions<WorkerSettings> options) : base(httpClient, options)
    {
        _httpClient = httpClient;
        _domain = options.Value.ApiBaseUrl;
    }

    public async Task<DocumentGenerationTask> GetNextTaskAsync(CancellationToken ct)
    {
        return await PostResponseAsync<object, DocumentGenerationTask>(null, $"{_BaseUrl}/next", ct);
    }

    private async Task<TemplateInfo?> DownloadTemplateAsync(string uri, CancellationToken ct)
    {
        var response = await _httpClient.GetAsync(_domain + uri, ct);
        response.EnsureSuccessStatusCode();

        var contentDisposition = response.Content.Headers.ContentDisposition;
        // Отдаем приоритет FileNameStar для корректной обработки не-ASCII символов (кириллицы)
        // и убираем кавычки, которые могут присутствовать в FileName.
        string? fileName = contentDisposition?.FileNameStar ?? contentDisposition?.FileName?.Trim('"');

        if (string.IsNullOrEmpty(fileName))
        {
            // Fallback or error if filename is not available
            fileName = Guid.NewGuid().ToString() + ".tmp";
        }

        var fileBytes = await response.Content.ReadAsByteArrayAsync(ct);
        return new TemplateInfo
        {
            FileName = fileName,
            FileContent = fileBytes
        };
    }

    public async Task<TemplateInfo?> GetTemplateAsync(int templateId, CancellationToken ct)
    {
        return await DownloadTemplateAsync($"{_BaseUrl}/{templateId}/template", ct);
    }

    public async Task SendCompleteStatusAsync(WorkerTaskCompletedDto request, Guid taskId, CancellationToken ct)
    {
        await PostResponseAsync<object, object>(request, $"{_BaseUrl}/{taskId}/complete", ct);
    }

    public async Task SendFailStatusAsync(WorkerTaskFailedDto request, Guid taskId, CancellationToken ct)
    {
        await PostResponseAsync<object, object>(request, $"{_BaseUrl}/{taskId}/fail", ct);
    }

    public async Task UploadDocumentToServerAsync(UploadDocumentDto dto, CancellationToken ct)
    {
        using var form = new MultipartFormDataContent();

        form.Add(new StringContent(dto.Title), "Title");
        form.Add(new StringContent(dto.CreatedBy.ToString()), "CreatedBy");
        form.Add(new StringContent(dto.TemplateId.ToString()), "TemplateId");
        form.Add(new StringContent(dto.DocumentType.ToString()), "DocumentType");

        var fileContent = new ByteArrayContent(dto.Content);
        fileContent.Headers.ContentType =
            new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

        // "File" — имя поля, должно совпадать с [FromForm(Name="File")]
        // dto.FileName — имя файла, которое увидит сервер (IFormFile.FileName)
        form.Add(fileContent, "File", dto.FileName);

        using var response = await _httpClient.PostAsync("/api/document/upload", form, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task SendProgressAsync(WorkerTaskProgressDto request, Guid taskId, CancellationToken ct)
    {
        await PostResponseAsync<WorkerTaskProgressDto, object>(request, $"{_BaseUrl}/{taskId}/progress", ct);
    }
}
