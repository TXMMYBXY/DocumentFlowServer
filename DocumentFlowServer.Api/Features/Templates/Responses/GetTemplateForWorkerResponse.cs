using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.Templates.Responses;

public class GetTemplateForWorkerResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("path")]
    public string Path { get; set; }
}