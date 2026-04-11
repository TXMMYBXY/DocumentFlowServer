namespace DocumentFlowServer.Application.Services.Authorization;

public interface IRefreshTokenHasher
{
    string Hash(string refreshToken);
}
