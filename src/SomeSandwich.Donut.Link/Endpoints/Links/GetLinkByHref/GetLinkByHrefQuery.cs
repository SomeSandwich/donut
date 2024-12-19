namespace SomeSandwich.Donut.Link.Endpoints.Links.GetLinkByHref;

/// <summary>
/// Query to get a link by its Href.
/// </summary>
public class GetLinkByHrefQuery
{
    /// <summary>
    /// The Href of the link to be retrieved.
    /// </summary>
    public required string Href { get; set; }
}
