
using ApplicationLayer.Reposatories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ApplicationLayer.Services;

public class CacheService(IDistributedCache distributedCache,ILogger<CacheService> logger) : ICachService
{
    private readonly IDistributedCache _distributedCache = distributedCache;
    private readonly ILogger<CacheService> _logger = logger;

    public  async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        _logger.LogInformation("Get {key}", key);
        var cachValue = await _distributedCache.GetStringAsync(key, cancellationToken);
        return String.IsNullOrEmpty(cachValue) 
            ? null
            : JsonSerializer.Deserialize<T>(cachValue);
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        _logger.LogInformation("Set {key}", key);

        await _distributedCache.SetStringAsync(key,JsonSerializer.Serialize(value),cancellationToken);
    }
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default) 
    {
        _logger.LogInformation("Remove {key}", key);

        await _distributedCache.RemoveAsync(key, cancellationToken);
    }
}
