using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.Donut.Application.Common.Constants;
using SomeSandwich.Donut.Application.Common.Interfaces;
using SomeSandwich.Donut.Application.Common.Utils;
using SomeSandwich.Donut.Link.Endpoints.Links.GenerateLinkMetadata;
using ZiggyCreatures.Caching.Fusion;

namespace SomeSandwich.Donut.Link.Endpoints.Links.CreateLink;

/// <summary>
/// Endpoint for creating a new link.
/// </summary>
public class CreateLinkEndpoint : IEndpoint
{
    /// <inheritdoc />
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/link", async (
            ILogger<CreateLinkCommand> logger,
            IMongoClient db,
            IFusionCache cache,
            IBus queue,
            [FromBody] CreateLinkCommand command,
            CancellationToken cancellationToken) =>
        {
            var linkCollection = db.GetMongoCollection<Domain.Link.Link>("links");

            var isExist = await (await linkCollection.FindAsync(l => l.Href == command.Href, null, cancellationToken))
                .FirstOrDefaultAsync(cancellationToken);

            if (isExist is not null)
            {
                logger.LogInformation("Href {Href} already exists.", command.Href);
                throw new DomainException($"Link \"{command.Href}\" already exists in the system.");
            }

            var id = ObjectId.GenerateNewId();
            var link = new Domain.Link.Link { Id = id, Href = command.Href };
            await linkCollection.InsertOneAsync(link, null, cancellationToken);

            await queue.Publish(new GenerateLinkMetadataCommand { Id = id, Href = command.Href }, cancellationToken);

            await cache.SetAsync($"link:{link.Href}", link, token: cancellationToken);

            logger.LogInformation("Create new link with {Href}", command.Href);

            return link;
        })
        .AddEndpointMetadata<Domain.Link.Link>(EndpointTagConstants.LinksTag, "Create new link.");
    }
}
