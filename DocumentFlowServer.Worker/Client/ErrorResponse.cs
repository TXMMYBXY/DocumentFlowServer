using System.Text.Json.Serialization;

namespace DocumentFlowServer.Worker.Client;

public class ErrorResponse
{
    [JsonPropertyName("Message")]
    public string Message { get; set; } = string.Empty;
}
