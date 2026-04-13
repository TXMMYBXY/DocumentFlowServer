using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Controllers.User.ViewModels;

public class PagedUserViewModel : PagedData
{
    [JsonPropertyName("users")]
    public List<GetUserViewModel> Users { get; set; }
}
