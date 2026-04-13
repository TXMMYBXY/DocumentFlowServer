using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.Authorization.Responses;

public class LoginResponse
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }
}