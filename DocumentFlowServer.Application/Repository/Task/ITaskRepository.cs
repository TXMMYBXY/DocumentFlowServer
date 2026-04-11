using DocumentFlowServer.Entities.Models;

namespace DocumentFlowServer.Application.Repository.Task;

public interface ITaskRepository : IBaseRepository<TaskModel>
{
    Task<TaskModel?> GetTaskByIdAsync(Guid taskId);
    Task<TaskModel?> GetTaskByStatusPendingAsync();
}
