using Aminoce.Models.Settings;
using Aminoce.Services.Logging;
using Aminoce.Services.Network;
using Aminoce.Services.Network.Apis;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aminoce;

public class AppBuilder
{
    private readonly HostApplicationBuilder _hostAppBuilder;

    public AppBuilder()
    {
        _hostAppBuilder = new HostApplicationBuilder();
        _hostAppBuilder.Logging.ClearProviders();
        _hostAppBuilder.Logging.AddProvider(new AppLoggerProvider(_hostAppBuilder.Configuration));

        _hostAppBuilder.Services.AddSingleton<HttpServer>();
        _hostAppBuilder.Services.AddSingleton<ApiController>();
        _hostAppBuilder.Services.AddSingleton<AuthGate>();

        _hostAppBuilder.Services.Configure<NetworkSettings>(
            options => _hostAppBuilder.Configuration.GetSection("Network").Bind(options)
        );
    }

    public App Build() => new(_hostAppBuilder.Build());
}