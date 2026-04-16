using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.Templates.Requests;

public class UpdateTemplateRequest
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("file")]
    public IFormFile? File { get; set; }
}