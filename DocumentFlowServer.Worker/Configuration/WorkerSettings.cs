namespace DocumentFlowServer.Worker.Configuration;

public class WorkerSettings
{
    public string ApiBaseUrl { get; set; } = "http://localhost:5189/api";
    public TimeSpan PollInterval { get; set; } = TimeSpan.FromSeconds(10);
    public int MaxRetries { get; set; } = 3;
    public string ApiKey { get; set; } = "secretkeyforworker";
}