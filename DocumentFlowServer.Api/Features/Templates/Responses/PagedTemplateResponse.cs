using System.Text.Json.Serialization;
using DocumentFlowServer.Application.Common;
using DocumentFlowServer.Application.Template.Dtos;

namespace DocumentFlowServer.Api.Features.Templates.Responses;

public class PagedTemplateResponse : PagedData
{
    [JsonPropertyName("templates")]
    public List<TemplateDto> Templates { get; set; }
}