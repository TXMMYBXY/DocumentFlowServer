using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Controllers.User.ViewModels;

public class DeleteManyUsersViewModel
{
    [JsonPropertyName("userIds")]
    public List<int> UserIds { get; set; }
}
