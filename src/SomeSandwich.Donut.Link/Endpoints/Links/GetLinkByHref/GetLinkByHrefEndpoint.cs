﻿using MongoDB.Driver;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.Donut.Application.Common.Constants;
using SomeSandwich.Donut.Application.Common.Interfaces;
using SomeSandwich.Donut.Application.Common.Utils;
using ZiggyCreatures.Caching.Fusion;

namespace SomeSandwich.Donut.Link.Endpoints.Links.GetLinkByHref;

/// <summary>
/// Endpoint to get a link by its Href.
/// </summary>
public class GetLinkByHrefEndpoint : IEndpoint
{
    /// <inheritdoc />
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/link", async (
            ILogger<GetLinkByHrefEndpoint> logger,
            IMongoClient db,
            IFusionCache cache,
            [AsParameters] GetLinkByHrefQuery query,
            CancellationToken cancellationToken) =>
        {
            var href = query.Href.RemoveWww();

            var linkCollection = db.GetMongoCollection<Domain.Link.Link>("links");
            var link = await cache.GetOrSetAsync<Domain.Link.Link>($"link:{href}", async _ =>
            {
                return await (await linkCollection.FindAsync(l => l.Href == href, cancellationToken: cancellationToken))
                    .FirstOrDefaultAsync(cancellationToken);
            }, token: cancellationToken);

            if (link is null)
            {
                logger.LogInformation("Not found link {Href}", href);
                throw new NotFoundException($"Not found link \"{href}\" in system.");
            }

            logger.LogDebug("Found link {Href} in system.", href);

            return link;
        })
        .AddEndpointMetadata<Domain.Link.Link>(EndpointTagConstants.LinksTag, "Get link by Href.");
    }
}
