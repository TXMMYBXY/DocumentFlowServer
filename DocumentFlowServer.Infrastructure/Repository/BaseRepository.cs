using DocumentFlowServer.Application.Reposiroty;
using DocumentFlowServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowServer.Infrastructure.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }
    
    public async Task<ICollection<T>?> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<int> GetCountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _dbSet
            .Where(e => EF.Property<int>(e, "Id") == id)
            .ExecuteDeleteAsync();
    }

    public async Task DeleteManyAsync(ICollection<int> ids)
    {
        await _dbContext.Set<T>()
            .Where(e => ids.Contains((int)e.GetType().GetProperty("Id").GetValue(e)))
            .ExecuteDeleteAsync();
    }

    public async Task<bool> ExistsAsync()
    {
        return await _dbSet.AnyAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}