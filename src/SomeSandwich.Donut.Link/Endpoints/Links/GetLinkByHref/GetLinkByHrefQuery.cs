using Microsoft.AspNetCore.Mvc;
using SomeSandwich.Donut.Abstractions.OpenApi;
using SomeSandwich.Donut.Abstractions.OpenApi.Attributes;

namespace SomeSandwich.Donut.Link.Endpoints.Links.GetLinkByHref;

/// <summary>
/// Query to get a link by its Href.
/// </summary>
[OpenApiExample<GetLinkByHrefQuery>]
public class GetLinkByHrefQuery : IExampleProvider<GetLinkByHrefQuery>
{
    /// <summary>
    /// The Href of the link to be retrieved.
    /// </summary>
    [FromQuery]
    public required string Href { get; set; }

    /// <inheritdoc />
    public static GetLinkByHrefQuery GenerateExample()
    {
        return new GetLinkByHrefQuery { Href = "https://www.example.com" };
    }
}
