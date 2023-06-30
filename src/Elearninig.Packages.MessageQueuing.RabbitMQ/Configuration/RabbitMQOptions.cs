namespace Elearninig.Packages.MessageQueuing.RabbitMQ.Configuration;

public sealed class RabbitMQOptions
{
    /// <summary>
    /// Username to access to rabbitmq
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Password to access to rabbitmq
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// VirtualHost of rabbitmq, Default '/'
    /// </summary>
    public string? VirtualHost { get; set; }

    /// <summary>
    /// Host rabbitmq running on
    /// </summary>
    public string? Host { get; set; }

    /// <summary>
    /// Port rabbitmq running on
    /// </summary>
    public int? Port { get; set; }

    /// <summary>
    /// Number of retries , Default 5
    /// </summary>
    public int NumberOfRetry { get; set; } = 5;

    /// <summary>
    /// Use exponential retry, Default false
    /// </summary>
    public bool UseExponential { get; set; } = false;

    /// <summary>
    /// Use scheduled redelivery retry, Default false
    /// </summary>
    public bool UseScheduledRedelivery { get; set; } = false;
}