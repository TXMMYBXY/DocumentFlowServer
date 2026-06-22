using DocumentFlowServer.Worker.Client.Api.Dto;
using DocumentFlowServer.Worker.Configuration;
using DocumentFlowServer.Worker.Interface;
using DocumentFlowServer.Worker.Interface.Client;
using DocumentFlowServer.Worker.Models;
using Microsoft.Extensions.Options;

namespace DocumentFlowServer.Worker.Service;

public class WorkerService : BackgroundService
{
    private readonly ILogger<WorkerService> _logger;
    private readonly IDocumentTemplateService _templateService;
    private readonly WorkerSettings _settings;
    private readonly IApiClient _apiClient;

    public WorkerService(
        ILogger<WorkerService> logger,
        IDocumentTemplateService templateService,
        IOptions<WorkerSettings> settings,
        IApiClient apiClient)
    {
        _logger = logger;
        _templateService = templateService;
        _settings = settings.Value;
        _apiClient = apiClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Document Flow Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var task = await _GetPendingTaskAsync(stoppingToken);

                if (task != null)
                {
                    _logger.LogInformation("Processing task {TaskId} for template {TemplateId}",
                        task.TaskId, task.TemplateId);

                    // Обновляем статус на "Processing"
                    await _UpdateTaskStatusAsync(task.TaskId, Models.TaskStatus.Processing, stoppingToken);

                    // Обрабатываем задачу
                    var result = await _ProcessTaskAsync(task, stoppingToken);

                    // Сохраняем результат (Completed или Failed)
                    await _SaveTaskResultAsync(task.TaskId, result, stoppingToken);
                }
                else
                {
                    // Если задач нет, ждем
                    await Task.Delay(_settings.PollInterval, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in worker execution cycle");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
    
    private async Task<DocumentGenerationTask?> _GetPendingTaskAsync(CancellationToken ct)
    {
        try
        {
            var response = await _apiClient.GetNextTaskAsync(ct);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get pending task");

            return null;
        }
    }

    private async Task<ProcessResult> _ProcessTaskAsync(DocumentGenerationTask task, CancellationToken ct)
    {
        TemplateInfo? template = null;
        try
        {
            template = await _apiClient.GetTemplateAsync(task.TemplateId, ct);

            if (template != null)
            {
                // Создаем уникальный путь для временного файла, сохраняя исходное расширение
                var tempFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_{template.FileName}");
                await File.WriteAllBytesAsync(tempFilePath, template.FileContent, ct);
                template.TempFilePath = tempFilePath;
            }

            if (template == null || string.IsNullOrEmpty(template.TempFilePath))
                throw new InvalidOperationException("Template not found or invalid");

            // Преобразуем данные задачи в словарь <string, string>
            var stringData = task.Data.ToDictionary(
                kv => kv.Key,
                kv => kv.Value?.ToString() ?? string.Empty
            );

            // Заполняем шаблон
            var filledBytes = await _templateService.FillTemplateAsync(template.TempFilePath, stringData);

            var outputFileName =
                $"{Path.GetFileNameWithoutExtension(template.FileName)}_заполнен_{DateTime.UtcNow:yyyy-MM-dd-hh-mm}.docx";

            await _apiClient.UploadDocumentToServerAsync(new UploadDocumentDto
            {
                Title = outputFileName,
                CreatedBy = task.UserId,
                TemplateId = task.TemplateId,
                DocumentType = task.TemplateType,
                FileName = outputFileName,
                Content = filledBytes
            }, ct);

            return new ProcessResult
            {
                Success = true,
                FilePath = outputFileName, // Используем только имя файла для передачи в API
                FileName = outputFileName,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process task {TaskId}", task.TaskId);
            return new ProcessResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
        finally
        {
            // Очистка временного файла
            if (template is not null && !string.IsNullOrEmpty(template.TempFilePath) && File.Exists(template.TempFilePath))
            {
                File.Delete(template.TempFilePath);
            }
        }
    }

    /// <summary>
    /// Обновление статуса задачи на "Processing" через POST /progress
    /// </summary>
    private async Task _UpdateTaskStatusAsync(Guid taskId, Models.TaskStatus status, CancellationToken ct)
    {
        var request = new WorkerTaskProgressDto();

        await _apiClient.SendProgressAsync(request, taskId, ct);
    }

    /// <summary>
    /// Сохраняем результат задачи через POST /complete или /fail
    /// </summary>
    private async Task _SaveTaskResultAsync(Guid taskId, ProcessResult result, CancellationToken ct)
    {
        if (result.Success)
        {
            var request = new WorkerTaskCompletedDto { ResultFilePath = result.FilePath };

            await _apiClient.SendCompleteStatusAsync(request, taskId, ct);
        }
        else
        {
            var request = new WorkerTaskFailedDto { ErrorMessage = result.ErrorMessage };

            await _apiClient.SendFailStatusAsync(request, taskId, ct);
        }
    }
}