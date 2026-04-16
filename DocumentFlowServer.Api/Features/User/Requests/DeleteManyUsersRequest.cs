using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.User.Requests;

public class DeleteManyUsersRequest
{
    [Required]
    [JsonPropertyName("userIds")]
    public List<int> UserIds { get; set; }
}