namespace DocumentFlowServer.Application.Common.Services;

/// <summary>
/// Service for working with passwords
/// </summary>
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string passwordHash, string password);
}