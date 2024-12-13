using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SomeSandwich.Donut.Application.Common.Startup.OpenApi;

/// <summary>
/// A class representing an example provider for <see cref="ProblemDetails"/>.
/// </summary>
public class ProblemDetailsExampleProvider : IExampleProvider<ProblemDetails>
{
    /// <inheritdoc />
    public static ProblemDetails GenerateExample()
    {
        return new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Bad Request",
            Status = StatusCodes.Status400BadRequest,
            Detail = "The specified value is invalid.",
        };
    }
}
