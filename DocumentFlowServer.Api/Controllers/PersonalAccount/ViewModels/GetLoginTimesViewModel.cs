using System.Text.Json.Serialization;

namespace DocumentFlowServer.Api.Controllers.PersonalAccount.ViewModels;

public class GetLoginTimesViewModel
{
    [JsonPropertyName("loginTime")]
    public DateTime? LoginTime { get; set; }
}