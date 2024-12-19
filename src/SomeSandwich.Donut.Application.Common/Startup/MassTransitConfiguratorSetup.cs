using System.Reflection;
using MassTransit;
using SomeSandwich.Donut.Abstractions.JsonConverters;

namespace SomeSandwich.Donut.Application.Common.Startup;

/// <summary>
/// Configures MassTransit with RabbitMQ using provided configuration settings.
/// </summary>
public class MassTransitConfiguratorSetup
{
    private readonly IDictionary<string, string> configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="MassTransitConfiguratorSetup"/> class.
    /// </summary>
    /// <param name="configuration">The configuration settings for MassTransit.</param>
    public MassTransitConfiguratorSetup(IDictionary<string, string> configuration)
    {
        this.configuration = configuration;
    }

    /// <summary>
    /// Sets up the MassTransit bus with RabbitMQ using the provided configurator.
    /// </summary>
    /// <param name="configurator">The bus registration configurator.</param>
    public void Setup(IBusRegistrationConfigurator configurator)
    {
        configurator.SetKebabCaseEndpointNameFormatter();
        configurator.AddConsumers(Assembly.GetEntryAssembly());

        var port = ushort.Parse(configuration["Port"]);
        configurator.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(configuration["Host"], port, configuration["VHost"], h =>
            {
                h.Username(configuration["Username"]);
                h.Password(configuration["Password"]);

                cfg.ConfigureEndpoints(context);

                cfg.ConfigureJsonSerializerOptions(new JsonSerializerOptionSetup().Setup);
            });
        });
    }
}
