using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Controllers.Template.ViewModels;

public class DeleteManyTemplatesViewModel
{
    [JsonPropertyName("templateIds")]
    public List<int> TemplateIds { get; set; } = new();
}
