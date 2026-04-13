using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.User.Requests;

public class SetUserPasswordRequest
{
    [Required]
    [JsonPropertyName("password")]
    public string Password { get; set; }
}