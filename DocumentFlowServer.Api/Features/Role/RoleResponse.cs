using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.Role;

public class RoleResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}