using DocumentFlowServer.Application.Repository.Task;
using DocumentFlowServer.Application.Repository.Task.Dto;
using DocumentFlowServer.Entities.Models;
using DocumentFlowServer.Infrastructure.Data;
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

    public async Task<List<TaskEntity>> GetAllTasksAsync()
    {
        return await _dbContext.Tasks
            .Select(t => new TaskEntity(t)).ToListAsync();
    }
}
