using DocumentFlowServer.Worker.Client.Api.Dto;
using DocumentFlowServer.Worker.Models;

namespace DocumentFlowServer.Worker.Interface.Client;

public interface IApiClient : IGeneralClient
{
    Task<DocumentGenerationTask> GetNextTaskAsync(CancellationToken ct);
    Task<TemplateInfo?> GetTemplateAsync(int templateId, CancellationToken ct);
    Task SendProgressAsync(WorkerTaskProgressDto request, Guid taskId, CancellationToken ct);
    Task SendCompleteStatusAsync(WorkerTaskCompletedDto request, Guid taskId, CancellationToken ct);
    Task SendFailStatusAsync(WorkerTaskFailedDto request, Guid taskId, CancellationToken ct);
    Task UploadDocumentToServerAsync(UploadDocumentDto dto, CancellationToken ct);
}
