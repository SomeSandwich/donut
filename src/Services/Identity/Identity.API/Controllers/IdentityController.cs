using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

/// <summary>
/// Represents the controller for managing identity related operations.
/// </summary>
[ApiController]
[Route("api/identity")]
public class IdentityController : ControllerBase
{
    private readonly ILogger<IdentityController> logger;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    public IdentityController(ILogger<IdentityController> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves the identity information.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identity response.</returns>
    [HttpGet("")]
    public async Task<ActionResult<GetIdentityResponse>> GetIdentity(CancellationToken cancellationToken)
    {
        return new GetIdentityResponse
        {
            Identity = "Identity.API"
        };
    }

    /// <summary>
    /// Represents the response for the GetIdentity method.
    /// </summary>
    public class GetIdentityResponse
    {
        /// <summary>
        /// Gets or sets the identity value.
        /// </summary>
        required public string Identity { get; set; }
    }
}
