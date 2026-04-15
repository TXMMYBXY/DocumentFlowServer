using System.Security.Cryptography;
using System.Text;
using DocumentFlowServer.Application.Common.Configuration;
using DocumentFlowServer.Application.RefreshToken;
using Microsoft.Extensions.Options;

namespace DocumentFlowServer.Infrastructure.RefreshToken;

public class RefreshTokenHasher : IRefreshTokenHasher
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