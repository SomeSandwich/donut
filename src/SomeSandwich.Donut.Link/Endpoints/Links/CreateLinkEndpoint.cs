using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SomeSandwich.Donut.Application.Common.Interfaces;

namespace SomeSandwich.Donut.Link.Endpoints.Links;

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
            IMongoClient dbClient,
            [FromBody] CreateLinkCommand command,
            CancellationToken cancellationToken) =>
        {
            var linkDb = dbClient.GetDatabase("LinkDB");
            var linksCollection = linkDb.GetCollection<Domain.Link.Link>("links");

            await linksCollection.InsertOneAsync(new Domain.Link.Link { Href = command.Url }, null, cancellationToken);

            logger.LogInformation("Create new link with {Url}", command.Url);
        });
    }
}
