using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.Document.Responses;

public class DownloadDocumentResponse
{
    [JsonPropertyName("filePath")]
    public string FilePath { get; set; }

    [JsonPropertyName("fileName")]
    public string FileName { get; set; }
}