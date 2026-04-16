namespace DocumentFlowServer.Application.RefreshToken;

/// <summary>
/// Service for hashing token
/// </summary>
public interface IRefreshTokenHasher
{
    string Hash(string refreshToken);
}