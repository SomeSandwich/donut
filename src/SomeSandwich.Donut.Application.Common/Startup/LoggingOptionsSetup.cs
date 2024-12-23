using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Events;

namespace SomeSandwich.Donut.Application.Common.Startup;

/// <summary>
/// Logging setup for application.
/// </summary>
public class LoggingOptionsSetup
{
    private readonly IConfiguration configuration;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="configuration">Configuration.</param>
    public LoggingOptionsSetup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    /// <summary>
    /// Setup logging.
    /// </summary>
    /// <param name="serviceProvider">Application service provider.</param>
    /// <param name="loggerConfiguration">Logger configuration.</param>
    public void Setup(IServiceProvider serviceProvider, LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(serviceProvider)
            .Enrich.FromLogContext()
            .WriteTo.Console();
    }

    /// <summary>
    /// Configure request logging options.
    /// </summary>
    /// <param name="options">Options configuration object.</param>
    public void SetupRequestLoggingOptions(RequestLoggingOptions options)
    {
        options.GetLevel = (httpContext, elapsed, ex) =>
        {
            // Default implementation copied from https://github.com/serilog/serilog-aspnetcore/blob/dev/src/Serilog.AspNetCore/AspNetCore/RequestLoggingOptions.cs
            if (ex != null)
            {
                return LogEventLevel.Error;
            }
            if (httpContext.Response.StatusCode > 499)
            {
                return LogEventLevel.Error;
            }

            return LogEventLevel.Information;
        };
    }
}
