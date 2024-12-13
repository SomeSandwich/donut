using SomeSandwich.Donut.Application.Common.Interfaces;

namespace SomeSandwich.Donut.Identity.Endpoints.Weathers;

/// <summary>
/// Endpoint for searching weather forecasts.
/// </summary>
public class SearchWeathers : IEndpoint
{
    /// <inheritdoc />
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/weatherforecast", () =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                .ToArray();
            return forecast;
        })
        .WithTags(EndpointTagConstants.Weathers)
        .WithSummary("Search Weathers")
        .Produces<WeatherForecast[]>();
    }
}
