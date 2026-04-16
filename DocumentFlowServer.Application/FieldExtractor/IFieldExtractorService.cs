using DocumentFlowServer.Application.FieldExtractor.Dtos;

namespace DocumentFlowServer.Application.FieldExtractor;

public interface IFieldExtractorService
{
    Task<List<TemplateFieldInfoDto>> ExtractFieldsAsync(string templatePath);
}