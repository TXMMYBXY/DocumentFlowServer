namespace DocumentFlowServer.Infrastructure.Configuration;

public class RefreshTokenSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiresDays { get; set; }
}