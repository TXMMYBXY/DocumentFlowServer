using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.Personal.Responses;

public class LoginTimeResponse
{
    [JsonPropertyName("loginTime")]
    public DateTime? LoginTime { get; set; }
}