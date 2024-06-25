using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Aminoce.Models.Network;
using Aminoce.Models.Settings;
using Aminoce.Services.Logging;
using Aminoce.Services.Network.Apis;

using EmbedIO;
using EmbedIO.WebApi;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Aminoce.Services.Network;

public class HttpServer : IHostedService
{
    private readonly WebServer _webServer;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ILogger<HttpServer> _logger;
    private readonly IOptions<NetworkSettings> _options;

    public HttpServer(
        ILogger<HttpServer> logger,
        IOptions<NetworkSettings> options,
        AuthGate authGate,
        ApiController apiController
    )
    {
        Swan.Logging.Logger.NoLogging();
        Swan.Logging.Logger.RegisterLogger(new SwanLogger(logger));

        _logger = logger;
        _options = options;
        _cancellationTokenSource = new();

        _webServer = new(CreateOptions());
        _webServer.WithModule(authGate);
        _webServer.WithWebApi("/v1", (module) => module.WithController(() => apiController));
        _webServer.WithAction("/", HttpVerbs.Any, HandleRootRequest);
        _webServer.OnUnhandledException = HandleException;
        _webServer.OnHttpException = HandleHttpException;
    }

    private WebServerOptions CreateOptions()
    {
        var options = new WebServerOptions();

        foreach (var url in _options.Value.UrlPrefixes)
            options.AddUrlPrefix(url);

        if (_options.Value.Certificate.Enable)
        {
            options.AutoLoadCertificate = _options.Value.Certificate.AutoLoadCertificate;
            options.AutoRegisterCertificate = _options.Value.Certificate.AutoRegisterCertificate;

            if (File.Exists(_options.Value.Certificate.Path))
                options.Certificate = new(
                    _options.Value.Certificate.Path,
                    _options.Value.Certificate.Password
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

    private async Task HandleException(IHttpContext httpContext, Exception e)
    {
        _logger.LogCritical(e, "[{}]", httpContext.Id);

        await httpContext.SendPacketAsync<object>(
            new(null, DataType.Unknown) { Message = $"{e.GetType()}: {e.Message}" },
            HttpStatusCode.InternalServerError
        );
    }

    private static async Task HandleHttpException(
        IHttpContext context,
        IHttpException httpException
    )
    {
        await context.SendPacketAsync<object>(
            new(null, DataType.Unknown)
            {
                Message =
                    (
                        httpException.Message
                        ?? HttpStatusDescription.Get(httpException.StatusCode)
                        ?? "Unknown"
                    ) + $", StatusCode={httpException.StatusCode}",
            },
            httpException.StatusCode
        );
    }

    private async Task HandleRootRequest(IHttpContext httpcontext)
    {
        await httpcontext.SendStringAsync(
            "Please visit https://github.com/Zaitonn/Aminoce for more information.",
            "text/plain",
            Encoding.UTF8
        );
    }
}