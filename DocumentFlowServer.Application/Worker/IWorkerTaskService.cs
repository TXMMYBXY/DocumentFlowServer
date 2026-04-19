using DocumentFlowServer.Application.Worker.Dtos;

namespace DocumentFlowServer.Application.Worker;

public interface IWorkerTaskService
{
    Task<WorkerTaskDto?> GetNextAsync(Guid workerId);
    Task UpdateProgressAsync(Guid taskId, WorkerTaskProgressDto dto);
    Task CompleteAsyncById(Guid taskId, WorkerTaskCompletedDto dto);
    Task FailAsyncById(Guid taskId, WorkerTaskFailedDto dto);
}