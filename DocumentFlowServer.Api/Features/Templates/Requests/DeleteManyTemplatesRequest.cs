using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.Templates.Requests;

public class DeleteManyTemplatesRequest
{
    [Required]
    [JsonPropertyName("templateIds")]
    public List<int> TemplateIds { get; set; } = new();
}