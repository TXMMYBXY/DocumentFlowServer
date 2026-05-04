using System.Text.Json.Serialization;

namespace DocumentFlowServer.Application.Template.Dtos;

public class GetTemplateForWorkerDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("path")]
    public string Path { get; set; }
}