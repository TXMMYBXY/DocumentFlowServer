namespace DocumentFlowServer.Application.Reposiroty;

public interface IBaseRepository<T> where T : class
{
    Task<ICollection<T>?> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<int> GetCountAsync();
    
    Task AddAsync(T entity);
    
    Task DeleteAsync(int id);
    Task DeleteManyAsync(ICollection<int> ids);
    
    Task<bool> ExistsAsync();
    
    Task SaveChangesAsync();
}   