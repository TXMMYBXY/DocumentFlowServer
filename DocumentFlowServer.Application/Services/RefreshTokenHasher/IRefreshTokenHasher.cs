namespace DocumentFlowServer.Application.Services.RefreshTokenHasher;

public interface IRefreshTokenHasher
{
    string Hash(string refreshToken);
}
