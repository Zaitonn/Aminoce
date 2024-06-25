using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Aminoce.Models.Settings;
using Aminoce.Services.Network;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Aminoce;

public class App(IHost host) : IHost
{
    public static readonly Version Version = typeof(App).Assembly.GetName().Version!;

    public static readonly string? InformationalVersion = Assembly
        .GetExecutingAssembly()
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
        ?.InformationalVersion;

    private readonly IHost _host = host;

    private readonly ILogger<App> _logger = host.Services.GetRequiredService<ILogger<App>>();

    IServiceProvider IHost.Services => _host.Services;

    public void Dispose()
    {
        _host.Dispose();
        GC.SuppressFinalize(this);
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting...");
        _logger.LogInformation("Version: {}", InformationalVersion ?? Version.ToString());
        _host.Services.GetRequiredService<HttpServer>().StartAsync(cancellationToken);
        if (
            _host.Services.GetRequiredService<IOptions<NetworkSettings>>().Value.AccessTokens.Length
            == 0
        )
            _logger.LogWarning("The setting item 'Network.AccessTokens' is empty.");
        return Task.Delay(-1, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Stopping...");

        return Task.CompletedTask;
    }
}
