using System.Reflection;
using MassTransit;

namespace SomeSandwich.Donut.Application.Common.Startup;

/// <summary>
/// Configures MassTransit with RabbitMQ using provided configuration settings.
/// </summary>
public class MassTransitConfiguratorSetup
{
    private readonly RabbitMqOptions options;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="options"></param>
    public MassTransitConfiguratorSetup(RabbitMqOptions options)
    {
        this.options = options;
    }

    /// <summary>
    /// Sets up the MassTransit bus with RabbitMQ using the provided configurator.
    /// </summary>
    /// <param name="configurator">The bus registration configurator.</param>
    public void Setup(IBusRegistrationConfigurator configurator)
    {
        configurator.SetKebabCaseEndpointNameFormatter();
        configurator.AddConsumers(Assembly.GetEntryAssembly());

        configurator.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(options.Host, options.Port, options.VHost, h =>
            {
                h.Username(options.Username);
                h.Password(options.Password);

                cfg.ConfigureEndpoints(context);

                cfg.ConfigureJsonSerializerOptions(new JsonSerializerOptionSetup().Setup);
            });
        });
    }
}
