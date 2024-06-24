using Aminoce.Services.Logging;
using Aminoce.Services.Server.Apis;

using EmbedIO;
using EmbedIO.WebApi;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aminoce.Services.Server;

public class HttpServer : IHostedService
{
    private readonly WebServer _webServer;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ILogger<HttpServer> _logger;
    private readonly IConfiguration _configuration;

    public HttpServer(
        ILogger<HttpServer> logger,
        IConfiguration configuration,
        ApiController apiController
    )
    {
        Swan.Logging.Logger.UnregisterLogger<Swan.Logging.ConsoleLogger>();
        Swan.Logging.Logger.RegisterLogger(new SwanLogger(logger));

        _logger = logger;
        _configuration = configuration;
        _cancellationTokenSource = new();

        _webServer = new(CreateOptions());
        _webServer.WithWebApi(
            "/api",
            (module) =>
                module
                    .WithController(() => apiController)
                    .HandleHttpException(ApiHelper.OnHttpException)
                    .HandleUnhandledException(ApiHelper.OnException)
        );
        _webServer.OnUnhandledException += HandleException;
    }

    private WebServerOptions CreateOptions()
    {
        var options = new WebServerOptions();
        var section = _configuration.GetSection("HttpServer");

        foreach (
            var url in section.GetValue<string[]>("UrlPrefixes")
                ?? throw new NullReferenceException("HttpServer:UrlPrefixes is not readable.")
        )
            options.AddUrlPrefix(url);

        if (section.GetValue<bool>("Certificate:Enable"))
        {
            options.AutoLoadCertificate = section.GetValue<bool>("Certificate:AutoLoadCertificate");
            options.AutoRegisterCertificate = section.GetValue<bool>(
                "Certificate:AutoRegisterCertificate"
            );

            if (File.Exists(section.GetValue<string>("Certificate:Path")))
                options.Certificate = new(
                    section.GetValue<string>("Certificate:Path")!,
                    section.GetValue<string>("Certificate:Password")
                );
        }

        return options;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _webServer.Start(_cancellationTokenSource.Token);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();
        _webServer.Dispose();

        return Task.CompletedTask;
    }

    private Task HandleException(IHttpContext httpContext, Exception e)
    {
        _logger.LogCritical(e, "[{}]", httpContext.Id);

        return Task.CompletedTask;
    }
}
