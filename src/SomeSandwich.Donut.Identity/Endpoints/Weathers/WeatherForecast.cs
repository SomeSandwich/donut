namespace SomeSandwich.Donut.Identity.Endpoints.Weathers;

/// <summary>
/// Represents a weather forecast with date, temperature, and summary information.
/// </summary>
public class WeatherForecast
{
    /// <summary>
    /// Gets or sets the date of the weather forecast.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Gets or sets the temperature in Celsius.
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// Gets the temperature in Fahrenheit.
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// Gets or sets the summary of the weather forecast.
    /// </summary>
    public string? Summary { get; set; }
}
