using System.Reflection;

using Aminoce.Services.Server;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aminoce;

public class AminoceApp(IHost host) : IHost
{
    public static readonly Version Version = typeof(AminoceApp).Assembly.GetName().Version!;

    public static readonly string? InformationalVersion = Assembly
        .GetExecutingAssembly()
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
        .InformationalVersion;

    private readonly IHost _host = host;

    private readonly ILogger<AminoceApp> _logger = host.Services.GetRequiredService<
        ILogger<AminoceApp>
    >();

    IServiceProvider IHost.Services => _host.Services;

    public void Dispose()
    {
        _host.Dispose();
        GC.SuppressFinalize(this);
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting...");
        _logger.LogInformation("Version: {}", InformationalVersion);
        _host.Services.GetRequiredService<HttpServer>().StartAsync(cancellationToken);

        return Task.Delay(-1, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Stopping...");

        return Task.CompletedTask;
    }
}
