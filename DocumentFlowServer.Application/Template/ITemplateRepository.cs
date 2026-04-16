using DocumentFlowServer.Application.Common.Reposiroty;
using DocumentFlowServer.Application.Template.Dtos;

namespace DocumentFlowServer.Application.Template;

public interface ITemplateRepository<T> : IBaseRepository<Entities.Models.DocumentTemplatesModels.Template> where T : Entities.Models.DocumentTemplatesModels.Template
{
    Task<ICollection<TemplateDto>> GetAllTemplatesAsync(TemplateFilter filter);
    Task<TemplateDto?> GetTemplateByIdAsync(int templateId);
    Task<bool> UpdateTemplateStatusAsync(int templateId);
    Task<string?> GetFilePathAsync(int templateId);
    Task UpdateTemplatePartialAsync(int templateId, string? templateDtoTitle, string? filePath);
}