using Serilog;

namespace Identity.API.Infrastructure.Startup;

/// <summary>
/// Configuration for Serilog.
/// </summary>
public class SerilogConfiguration
{
    private readonly IConfiguration configuration;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="configuration">The configuration object.</param>
    public SerilogConfiguration(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    /// <summary>
    /// Sets up the Serilog configuration.
    /// </summary>
    /// <param name="config">The logger configuration.</param>
    public void Setup(LoggerConfiguration config)
    {
        config.ReadFrom.Configuration(configuration);
        //config.ReadFrom()
    }
}
