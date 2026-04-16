namespace DocumentFlowServer.Api.Authorization.Policies;

public static class Policy
{
    public const string AdminOnly = "AdminOnly";
    public const string AdminAndBoss = "AdminAndBoss";
    public const string AdminBossAndPurchasher = "AdminBossAndPurchaser";
    public const string All = "All";
}