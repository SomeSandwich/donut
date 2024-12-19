using MassTransit;
using MongoDB.Bson;
using MongoDB.Driver;
using SomeSandwich.Donut.Application.Common.Utils;
using ZiggyCreatures.Caching.Fusion;

namespace SomeSandwich.Donut.Link.Endpoints.Links.GenerateLinkMetadata;

/// <summary>
/// Consumer that handles the generation of link metadata.
/// </summary>
public class GenerateLinkMetadataConsumer : IConsumer<GenerateLinkMetadataCommand>
{
    private readonly ILogger<GenerateLinkMetadataConsumer> logger;
    private readonly IMongoClient db;
    private readonly IFusionCache cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenerateLinkMetadataConsumer"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="db">The MongoDB client instance.</param>
    /// <param name="cache"></param>
    public GenerateLinkMetadataConsumer(ILogger<GenerateLinkMetadataConsumer> logger, IMongoClient db, IFusionCache cache)
    {
        this.logger = logger;
        this.db = db;
        this.cache = cache;
    }

    /// <summary>
    /// Consumes the <see cref="GenerateLinkMetadataCommand"/> to generate metadata for a link.
    /// </summary>
    /// <param name="context">The consume context containing the command message.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Consume(ConsumeContext<GenerateLinkMetadataCommand> context)
    {
        var command = context.Message;
        var uri = new Uri(command.Href);

        var metadata = new BsonDocument
        {
            { nameof(uri.Host), uri.Host },
            { "Uri", new BsonDocument
                {
                    { nameof(uri.AbsolutePath), uri.AbsolutePath },
                    { nameof(uri.Query), uri.Query }
                }
            }
        };

        var linkCollection = db.GetMongoCollection<Domain.Link.Link>("links");

        var update = Builders<Domain.Link.Link>.Update.Set(nameof(Domain.Link.Link.Metadata), metadata);
        if (command.Id is not null)
        {
            await linkCollection.UpdateOneAsync(l => l.Id == command.Id, update, null, CancellationToken.None);
        }
        else
        {
            await linkCollection.UpdateOneAsync(l => l.Href == command.Href, update, null, CancellationToken.None);
        }

        await cache.RemoveAsync($"link:{command.Href}");

        logger.LogInformation("Generate metadata for link {Href}", command.Href);
    }
}
