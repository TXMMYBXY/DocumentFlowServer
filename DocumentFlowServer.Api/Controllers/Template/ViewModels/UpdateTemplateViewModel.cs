using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Controllers.Template.ViewModels;

public class UpdateTemplateViewModel
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("file")]
    public IFormFile? File { get; set; }
}
