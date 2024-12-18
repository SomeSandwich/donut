using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.Donut.Application.Common.Constants;
using SomeSandwich.Donut.Application.Common.Interfaces;
using SomeSandwich.Donut.Application.Common.Utils;

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
            IMongoClient dbClient,
            [FromBody] CreateLinkCommand command,
            CancellationToken cancellationToken) =>
        {
            var linkCollection = dbClient.GetMongoCollection<Domain.Link.Link>("links");

            var isExist =
                await linkCollection.CountDocumentsAsync(l => l.Href == command.Url, null, cancellationToken);

            if (isExist > 0)
            {
                logger.LogInformation("Href {Href} already exists.", command.Url);
                throw new DomainException($"Link {command.Url} already exists in the system.");
            }

            var link = new Domain.Link.Link { Href = command.Url };
            await linkCollection.InsertOneAsync(link, null, cancellationToken);

            logger.LogInformation("Create new link with {Href}", command.Url);

            return isExist;
        })
        .AddEndpointMetadata<Domain.Link.Link>(EndpointTagConstants.LinksTag, "Create new link.");
    }
}
