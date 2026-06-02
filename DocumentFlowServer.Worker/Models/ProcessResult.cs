namespace DocumentFlowServer.Worker.Models;

public class ProcessResult
{
    public bool Success { get; set; }
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public string? ErrorMessage { get; set; }
}
