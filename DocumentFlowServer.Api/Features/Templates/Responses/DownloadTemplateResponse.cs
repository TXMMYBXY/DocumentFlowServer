using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.Templates.Responses;

public class DownloadTemplateResponse
{
    [JsonPropertyName("filePath")]
    public string FilePath { get; set; }

    [JsonPropertyName("fileName")]
    public string FileName { get; set; }
}