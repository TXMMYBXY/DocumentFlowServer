using DocumentFlowServer.Application.Common.Services;
using Microsoft.AspNetCore.Identity;

namespace DocumentFlowServer.Infrastructure.Common.Services;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password)
    {
        return _hasher.HashPassword(null, password);
    }

    public bool Verify(string passwordHash, string password)
    {
        var result = _hasher.VerifyHashedPassword(null, passwordHash, password);
        return result == PasswordVerificationResult.Success;
    }
}