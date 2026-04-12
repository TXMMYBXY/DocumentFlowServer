using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Controllers.Template.ViewModels;

public class DownloadTemplateViewModel
{
    [JsonPropertyName("filePath")]
    public string FilePath { get; set; }

    [JsonPropertyName("fileName")]
    public string FileName { get; set; }
}
