using System.Security.Cryptography;
using System.Text;
using DocumentFlowServer.Application.Services.RefreshTokenHasher;
using DocumentFlowServer.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace DocumentFlowServer.Infrastructure.Services;

public sealed class RefreshTokenHasher : IRefreshTokenHasher
{
    private readonly RefreshTokenSettings _refreshTokenSettings;

    public RefreshTokenHasher(IOptions<RefreshTokenSettings> refreshTokenSettings)
    {
        _refreshTokenSettings = refreshTokenSettings.Value;
    }

    public string Hash(string refreshToken)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_refreshTokenSettings.SecretKey));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));

        return Convert.ToBase64String(hashBytes);
    }
}
