namespace DocumentFlowServer.Application.Common.Configuration;

public class RefreshTokenSettings
{
    public string SecretKey { get; set; }
    public int ExpiresDays { get; set; }
}