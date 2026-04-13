using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Controllers.Authorization.ViewModels;

public class CreateAccessTokenViewModel
{
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
}
