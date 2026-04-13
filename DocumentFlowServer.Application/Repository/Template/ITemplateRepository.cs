using DocumentFlowServer.Application.Services.Template;
using DocumentFlowServer.Application.Services.Template.Dto;

namespace DocumentFlowServer.Application.Repository.Template;

public interface ITemplateRepository : IBaseRepository<Entities.Models.DocumentTemplatesModels.Template>
{
    /// <summary>
    /// Возвращает шаблонг по id
    /// </summary>
    Task<T> GetTemplateByIdAsync<T>(int templateId) where T : Entities.Models.DocumentTemplatesModels.Template;

    /// <summary>
    /// Возвращает список шаблонов по id
    /// </summary>
    Task<List<T>> GetAllTemplatesAsync<T>(TemplateFilter filter) where T : Entities.Models.DocumentTemplatesModels.Template;

    /// <summary>
    /// Создает новый шаблон в бд
    /// </summary>
    System.Threading.Tasks.Task CreateTemplateAsync<T>(T template) where T : Entities.Models.DocumentTemplatesModels.Template;

    /// <summary>
    /// Обновляет информацию о шаблоне в таблице
    /// </summary>
    System.Threading.Tasks.Task UpdateTemplatePartialAsync<T>(int templateId, string? title, string? path) where T : Entities.Models.DocumentTemplatesModels.Template;

    /// <summary>
    /// Обновляет статус шаблона в таблице
    /// </summary>
    Task<bool> UpdateTemplateStatusAsync<T>(int templateId) where T : Entities.Models.DocumentTemplatesModels.Template;

    /// <summary>
    /// Удаляет шаблон из таблицы
    /// </summary>
    System.Threading.Tasks.Task DeleteTemplateAsync<T>(int templateId) where T : Entities.Models.DocumentTemplatesModels.Template;

    /// <summary>
    /// Удалять несколько шаблонов из таблицы
    /// </summary>
    System.Threading.Tasks.Task DeleteManyTemplatesAsync<T>(List<int> templateIds) where T : Entities.Models.DocumentTemplatesModels.Template;

    /// <summary>
    /// Возвращает количество шаблонов в таблице
    /// </summary>
    Task<int> GetTotalCountAsync<T>() where T : Entities.Models.DocumentTemplatesModels.Template;

    Task<WorkerTemplateDto> GetWorkerTemplateByIdAsync<T>(int templateId) where T : Entities.Models.DocumentTemplatesModels.Template;

    Task<string> GetFilePathAsync<T>(int templateId)where T : Entities.Models.DocumentTemplatesModels.Template;
}