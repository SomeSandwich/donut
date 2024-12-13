using Scalar.AspNetCore;

namespace SomeSandwich.Donut.Application.Common.Startup;

/// <summary>
/// Provides setup configuration for Scalar options.
/// </summary>
public class ScalarOptionSetup
{
    /// <summary>
    /// Configures the specified Scalar options.
    /// </summary>
    /// <param name="options">The Scalar options to configure.</param>
    public void Setup(ScalarOptions options)
    {
        options
            .WithDownloadButton(true)
            .WithTheme(ScalarTheme.Alternate)
            .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios);
    }
}
