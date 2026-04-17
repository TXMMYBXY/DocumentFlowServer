using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Features.Personal.Requests;

public class ChangePasswordRequest
{
    [Required]
    [JsonPropertyName("currentPassword")]
    public string CurrentPassword { get; set; }
    
    [Required]
    [JsonPropertyName("newPassword")]
    public string NewPassword { get; set; }
    
    [Required]
    [JsonPropertyName("confirmPassword")]
    public string ConfirmPassword { get; set; }
}