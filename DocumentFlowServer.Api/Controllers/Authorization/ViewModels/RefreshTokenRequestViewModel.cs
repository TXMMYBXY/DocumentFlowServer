using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Controllers.Authorization.ViewModels;

public class RefreshTokenRequestViewModel
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
}
