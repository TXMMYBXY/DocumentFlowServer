using System.Linq.Expressions;
using DocumentFlowServer.Application.Repository;
using DocumentFlowServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext;
    protected readonly DbSet<T> _dbset;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbset = dbContext.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbset.AddAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _dbset
            .Where(e => EF.Property<int>(e, "Id") == id)
            .ExecuteDeleteAsync();
    }
    
    /// <summary>
    /// Метод для сохранения изменений в БД
    /// </summary>
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        _dbset.Update(entity);
    }

    public void UpdateFields(T entity, params Expression<Func<T, object>>[] fields)
    {
        _dbContext.Attach(entity);

        foreach (var field in fields)
        {
            _dbContext.Entry(entity).Property(field).IsModified = true;
        }
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _dbContext.Set<T>().CountAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task DeleteManyAsync(List<int> ids)
    {
        await _dbContext.Set<T>()
            .Where(e => ids.Contains((int)e.GetType().GetProperty("Id").GetValue(e)))
            .ExecuteDeleteAsync();
    }
}