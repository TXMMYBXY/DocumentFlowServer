using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Controllers.Template.ViewModels;

public class PagedTemplateViewModel : PagedData
{
    [JsonPropertyName("templates")]
    public List<GetTemplateViewModel> Templates { get; set; }
}
