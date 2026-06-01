using System.Text.Json.Serialization;

namespace DocumentFlowServer.Worker.Models;

public class DocumentGenerationTask
{
    [JsonPropertyName("taskId")]
    public Guid TaskId { get; set; }
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    [JsonPropertyName("templateId")]
    public int TemplateId { get; set; }
    [JsonPropertyName("templateType")]
    public TemplateType TemplateType { get; set; } 
    [JsonPropertyName("data")]
    public Dictionary<string, object> Data { get; set; } = new();
}