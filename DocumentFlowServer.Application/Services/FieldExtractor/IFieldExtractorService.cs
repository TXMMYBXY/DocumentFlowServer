using DocumentFlowServer.Application.Services.Template.Dto;

namespace DocumentFlowServer.Application.Services.FieldExtractor;

public interface IFieldExtractorService
{
    Task<List<TemplateFieldInfoDto>> ExtractFieldsAsync(string templatePath);
}
