using System.Text.Json.Serialization;

namespace DocumentFlowServer.Worker.Models;

public class TemplateInfo
{
    public string FileName { get; set; } = string.Empty;
    public byte[] FileContent { get; set; } = Array.Empty<byte>();
    public string TempFilePath { get; set; } = string.Empty;
}
