using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.Templates.Responses;

public class GetTemplateForWorkerResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("path")]
    public string FilePath { get; set; } = string.Empty;
}