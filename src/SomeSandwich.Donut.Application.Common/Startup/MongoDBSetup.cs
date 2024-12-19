using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;

namespace SomeSandwich.Donut.Application.Common.Startup;

/// <summary>
/// Provides extension methods for setting up MongoDB services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class MongoDBSetup
{
    /// <summary>
    /// Adds MongoDB services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="connectionString">The connection string to use for connecting to MongoDB.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddMongoDB(this IServiceCollection services, string connectionString)
    {
        var clientSettings = MongoClientSettings.FromConnectionString(connectionString);
        clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber(new InstrumentationOptions
        {
            CaptureCommandText = true
        }));

        services.AddSingleton<IMongoClient>(_ => new MongoClient(clientSettings));

        return services;
    }
}
