using System;

using Aminoce.Services.Network;

using Microsoft.Extensions.Logging;

namespace Aminoce.Services.Logging;

public class SwanLogger(ILogger<HttpServer> logger) : Swan.Logging.ILogger
{
    private readonly ILogger<HttpServer> _logger = logger;

    public Swan.Logging.LogLevel LogLevel { get; }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public void Log(Swan.Logging.LogMessageReceivedEventArgs logEvent)
    {
        var level = logEvent.MessageType switch
        {
            Swan.Logging.LogLevel.Info => Microsoft.Extensions.Logging.LogLevel.Information,
            Swan.Logging.LogLevel.Warning => Microsoft.Extensions.Logging.LogLevel.Warning,
            Swan.Logging.LogLevel.Error => Microsoft.Extensions.Logging.LogLevel.Error,
            Swan.Logging.LogLevel.Fatal => Microsoft.Extensions.Logging.LogLevel.Critical,
            Swan.Logging.LogLevel.Debug => Microsoft.Extensions.Logging.LogLevel.Debug,
            Swan.Logging.LogLevel.Trace => Microsoft.Extensions.Logging.LogLevel.Trace,
            Swan.Logging.LogLevel.None => Microsoft.Extensions.Logging.LogLevel.None,
            _ => Microsoft.Extensions.Logging.LogLevel.None
        };

        _logger.Log(level, logEvent.Exception, logEvent.Message);
    }
}