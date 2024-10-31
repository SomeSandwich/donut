using Microsoft.AspNetCore.Mvc;

namespace Link.API.Controllers;

/// <summary>
/// Represents a controller for managing links.
/// </summary>
[ApiController]
[Route("api/link")]
public class LinkController : ControllerBase
{
    private readonly ILogger<LinkController> logger;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public LinkController(ILogger<LinkController> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Gets the links.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An action result containing the links.</returns>
    [HttpGet("")]
    public async Task<ActionResult<GetLinksResponse>> GetLinks(CancellationToken cancellationToken)
    {
        return new GetLinksResponse { Link = "Link.API" };
    }

    /// <summary>
    /// Represents the response for getting links.
    /// </summary>
    public class GetLinksResponse
    {
        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        required public string Link { get; set; }
    }
}
