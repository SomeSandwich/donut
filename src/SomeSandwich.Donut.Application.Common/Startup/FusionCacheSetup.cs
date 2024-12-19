using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using SomeSandwich.Donut.Abstractions.JsonConverters;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace SomeSandwich.Donut.Application.Common.Startup;

/// <summary>
/// Provides extension methods to setup FusionCache with specific configurations.
/// </summary>
public static class FusionCacheSetup
{
    /// <summary>
    /// Configures the FusionCache builder with default entry options, a JSON serializer, and a distributed Redis cache.
    /// </summary>
    /// <param name="builder">The FusionCache builder to configure.</param>
    /// <param name="connectionString">The connection string for the Redis cache.</param>
    /// <returns>The configured FusionCache builder.</returns>
    public static IFusionCacheBuilder Setup(this IFusionCacheBuilder builder, string connectionString)
    {
        return builder
            .WithDefaultEntryOptions(new FusionCacheEntryOptions()
                .SetDuration(TimeSpan.FromMinutes(2))
                .SetPriority(CacheItemPriority.High)
                .SetFailSafe(true, TimeSpan.FromHours(2))
                .SetFactoryTimeouts(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(2)))
            .WithSerializer(new FusionCacheSystemTextJsonSerializer(new JsonSerializerOptions()
            {
                Converters = { new BsonDocumentJsonConverter(), new ObjectIdJsonConverter() }
            }))
            .WithDistributedCache(new RedisCache(new RedisCacheOptions { Configuration = connectionString }));
    }
}
