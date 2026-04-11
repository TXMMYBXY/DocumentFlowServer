using DocumentFlowServer.Application.Repository.Task;
using DocumentFlowServer.Entities.Data;
using DocumentFlowServer.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Repository;

public class TaskRepository : BaseRepository<TaskModel>, ITaskRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TaskRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TaskModel?> GetTaskByIdAsync(Guid taskId)
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);
    }

    public async Task<TaskModel?> GetTaskByStatusPendingAsync()
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Status == DocumentFlowServer.Entities.Enums.TaskStatus.Pending);
    }
}
