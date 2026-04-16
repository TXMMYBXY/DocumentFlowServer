using DocumentFlowServer.Application.FieldExtractor.Dtos;
using DocumentFlowServer.Application.Template.Dtos;

namespace DocumentFlowServer.Application.Template;

public interface ITemplateService<T> where T : Entities.Models.DocumentTemplatesModels.Template
{
    Task<PagedTemplateDto> GetAllTemplatesAsync(TemplateFilter filter);
    Task CreateTemplateAsync(CreateTemplateDto templateDto);
    Task<List<TemplateFieldInfoDto>> ExtractFieldsFromTemplateAsync(int templateId);

    Task UpdateTemplatePartialAsync(int templateId, UpdateTemplateDto templateDto);
    
    Task DeleteTemplateAsync(int templateId);

    Task<bool> ChangeTemplateStatusById(int templateId);
    
    Task<DownloadTemplateDto> DownloadTemplateAsync(int templateId);
}