using System.Linq.Expressions;

namespace DocumentFlowServer.Application.Repository;

public interface IBaseRepository<T> where T : class
{
    /// <summary>
    /// Добавление записи
    /// </summary>
    System.Threading.Tasks.Task AddAsync(T entity);
    
    /// <summary>
    /// Обновление записи
    /// </summary>
    void Update(T entity);

    /// <summary>
    /// Удаление записи
    /// </summary>
    System.Threading.Tasks.Task DeleteAsync(int id);

    /// <summary>
    /// Множественное удаление записей
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    System.Threading.Tasks.Task DeleteManyAsync(List<int> ids);

    /// <summary>
    /// Сохранение изменений в таблицах
    /// </summary>
    System.Threading.Tasks.Task SaveChangesAsync();

    /// <summary>
    /// Обновление записей в таблице
    /// </summary>
    /// <param name="entity">Сущность, у которой надо изменить свойства</param>
    /// <param name="fields">Свойства, которые меняются(в виде функции)</param>
    void UpdateFields(T entity, params Expression<Func<T, object>>[] fields);

    Task<int> GetTotalCountAsync();

    Task<T> GetByIdAsync(int id);
}