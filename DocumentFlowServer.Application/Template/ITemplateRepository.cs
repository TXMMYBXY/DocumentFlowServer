using DocumentFlowServer.Application.Common.Reposiroty;
using DocumentFlowServer.Application.Template.Dtos;
using DocumentFlowServer.Entities.Enums;

namespace DocumentFlowServer.Application.Template;

public interface ITemplateRepository : IBaseRepository<Entities.Models.Template>
{
    Task<(ICollection<TemplateDto>, int)> GetAllTemplatesAsync(TemplateFilter filter);
    Task<TemplateDto?> GetTemplateForDownloadingByIdAsync(int templateId);
    Task<bool> UpdateTemplateStatusAsync(int templateId);
    Task<string?> GetFilePathAsync(int templateId);
    Task UpdateTemplatePartialAsync(int templateId, string? templateDtoTitle, string? filePath);
    Task<TemplateType> GetTypeByTemplateIdAsync(int templateId);
    Task<List<GetTemplateDto>> GetTemplatesWithoutContractsAsync();
    Task<List<GetTemplateDto>> GetTemplatesAsync();
    Task AddManyTemplatesAsync(IEnumerable<Entities.Models.Template> templates);
}