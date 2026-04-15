namespace DocumentFlowServer.Application.RefreshToken;

public interface IRefreshTokenHasher
{
    string Hash(string refreshToken);
}