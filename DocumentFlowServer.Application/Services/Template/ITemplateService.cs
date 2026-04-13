using DocumentFlowServer.Application.Services.Template.Dto;

namespace DocumentFlowServer.Application.Services.Template;

public interface ITemplateService
{
    /// <summary>
    /// Получения шаблона по id
    /// </summary>
    Task<GetTemplateForWorkerDto> GetTemplateForWorkerByIdAsync<T>(int templateId) where T : Entities.Models.DocumentTemplatesModels.Template;

    /// <summary>
    /// Получение списка всех шаблонов
    /// </summary>
    Task<PagedTemplateDto> GetAllTemplatesAsync<T>(TemplateFilter templateFilter) where T : Entities.Models.DocumentTemplatesModels.Template;

    /// <summary>
    /// Создание нового шаблона
    /// </summary>
    Task CreateTemplateAsync<T>(CreateTemplateDto templateDto) where T : Entities.Models.DocumentTemplatesModels.Template, new();

    /// <summary>
    /// Изменения данных о шаблоне
    /// </summary>
    Task UpdateTemplatePartialAsync<T>(int templateId, UpdateTemplateDto templateDto) where T : Entities.Models.DocumentTemplatesModels.Template, new();

    /// <summary>
    /// Метод для удаления шаблона
    /// </summary>
    Task DeleteTemplateAsync<T>(int templateId) where T : Entities.Models.DocumentTemplatesModels.Template;

    /// <summary>
    /// Метод для смены статуса шаблона
    /// </summary>
    Task<bool> ChangeTemplateStatusById<T>(int templateId) where T : Entities.Models.DocumentTemplatesModels.Template;

    /// <summary>
    /// Метод для извелчения полей из шаблона
    /// </summary>
    Task<List<TemplateFieldInfoDto>> ExtractFieldsFromTemplateAsync<T>(int templateId) where T : Entities.Models.DocumentTemplatesModels.Template;

    /// <summary>
    /// Метод для удаления нескольких шаблонов
    /// </summary>
    Task DeleteManyTemplatesAsync<T>(List<int> templateIds) where T : Entities.Models.DocumentTemplatesModels.Template;

    Task<DownloadTemplateDto> DownloadTemplateAsync<T>(int templateId) where T : Entities.Models.DocumentTemplatesModels.Template, new();

    Task<IReadOnlyList<TemplateFieldInfoDto>> ExtractFieldsByAIFromTemplateAsync<T>(int templateId) where T : Entities.Models.DocumentTemplatesModels.Template;
}