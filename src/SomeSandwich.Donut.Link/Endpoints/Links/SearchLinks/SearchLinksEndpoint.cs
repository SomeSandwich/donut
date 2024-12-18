using MongoDB.Driver;
using SomeSandwich.Donut.Application.Common.Constants;
using SomeSandwich.Donut.Application.Common.Interfaces;
using SomeSandwich.Donut.Application.Common.Utils;
using SomeSandwich.Donut.Link.Endpoints.Links.CreateLink;

namespace SomeSandwich.Donut.Link.Endpoints.Links.SearchLinks;

/// <summary>
/// Endpoint for searching links.
/// </summary>
public class SearchLinksEndpoint : IEndpoint
{
    /// <inheritdoc />
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/links", async (
            ILogger<CreateLinkCommand> logger,
            IMongoClient dbClient,
            CancellationToken cancellationToken) =>
        {
            var linkCollection = dbClient.GetMongoCollection<Domain.Link.Link>("links");

            var links = await (await linkCollection.FindAsync(FilterDefinition<Domain.Link.Link>.Empty, null,
                    cancellationToken))
                .ToListAsync(cancellationToken);

            return links;
        })
        .AddEndpointMetadata<IList<Domain.Link.Link>>(EndpointTagConstants.LinksTag, "Search links.");
    }
}
