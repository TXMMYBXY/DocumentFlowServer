using System.Text.Json;
using DocumentFlowServer.Application.Role;
using DocumentFlowServer.Application.Role.Dtos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace DocumentFlowServer.Infrastructure.Role;

public class RoleService : IRoleService
{
    private const string RolesVersionKey = "roles_version";
    
    private readonly ILogger<RoleService> _logger;
    private readonly IDistributedCache _cache;
    
    private readonly IRoleRepository _repository;

    public RoleService(
        ILogger<RoleService> logger,
        IDistributedCache cache,
        IRoleRepository repository)
    {
        _logger = logger;
        _cache = cache;
        _repository = repository;
    }
    
    public async Task<ICollection<RoleDto>> GetRolesAsync()
    {
        _logger.LogInformation("getting roles");
        
        var version = await _GetUsersVersionAsync();
        var cacheKey = $"users_{version}";

        var cached = await _cache.GetStringAsync(cacheKey);

        if (cached != null)
        {
            return JsonSerializer.Deserialize<ICollection<RoleDto>>(cached);
        }

        var roles = await _repository.GetAllRolesAsync();
        
        var serializedResult = JsonSerializer.Serialize(roles);

        await _cache.SetStringAsync(cacheKey, serializedResult, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        });
        
        return roles;
    }
    
    private async Task<string> _GetUsersVersionAsync()
    {
        var version = await _cache.GetStringAsync(RolesVersionKey);

        if (version == null)
        {
            version = Guid.NewGuid().ToString();

            await _cache.SetStringAsync(RolesVersionKey, version);
        }

        return version;
    }
}