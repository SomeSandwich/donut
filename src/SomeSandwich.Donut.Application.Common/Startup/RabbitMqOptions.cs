namespace SomeSandwich.Donut.Application.Common.Startup;

/// <summary>
/// Represents the configuration options for connecting to a RabbitMQ server.
/// </summary>
public class RabbitMqOptions
{
    /// <summary>
    /// Gets or sets the hostname of the RabbitMQ server.
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Gets or sets the virtual host to use when connecting to the RabbitMQ server.
    /// </summary>
    public string VHost { get; set; }

    /// <summary>
    /// Gets or sets the username to use when connecting to the RabbitMQ server.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the password to use when connecting to the RabbitMQ server.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the port number to use when connecting to the RabbitMQ server.
    /// </summary>
    public ushort Port { get; set; }
}
