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
            var href = command.Href.RemoveWww();

            var linkCollection = db.GetMongoCollection<Domain.Link.Link>("links");
            var isExits = await cache.GetOrSetAsync<Domain.Link.Link>($"link:{href}", async _ =>
            {
                return await (await linkCollection.FindAsync(l => l.Href == href, cancellationToken: cancellationToken))
                    .FirstOrDefaultAsync(cancellationToken);
            }, token: cancellationToken);

            if (isExits is not null)
            {
                logger.LogInformation("Href {Href} already exists.", href);
                throw new DomainException($"Link \"{command.Href}\" already exists in the system.");
            }

            var id = ObjectId.GenerateNewId();
            var link = new Domain.Link.Link { Id = id, Href = href };
            await linkCollection.InsertOneAsync(link, null, cancellationToken);

            await queue.Publish(new GenerateLinkMetadataCommand { Id = id, Href = href }, cancellationToken);

            await cache.SetAsync($"link:{link.Href}", link, token: cancellationToken);

            logger.LogInformation("Create new link with {Href}", href);

            return link;
        })
        .AddEndpointMetadata<Domain.Link.Link>(EndpointTagConstants.LinksTag, "Create new link.");
    }
}
