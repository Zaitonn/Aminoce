using Aminoce.Services.Logging;
using Aminoce.Services.Server;
using Aminoce.Services.Server.Apis;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aminoce;

public class AminoceAppBuilder
{
    private readonly HostApplicationBuilder _hostAppBuilder;

    public AminoceAppBuilder()
    {
        _hostAppBuilder = new HostApplicationBuilder();
        _hostAppBuilder.Logging.ClearProviders();
        _hostAppBuilder.Logging.AddProvider(new AppLoggerProvider(_hostAppBuilder.Configuration));

        _hostAppBuilder.Services.AddSingleton<HttpServer>();
        _hostAppBuilder.Services.AddSingleton<ApiController>();
    }

    public AminoceApp Build() => new(_hostAppBuilder.Build());
}
