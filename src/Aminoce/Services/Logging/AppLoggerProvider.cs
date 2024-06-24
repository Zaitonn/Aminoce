using System.Collections.Concurrent;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Aminoce.Services.Logging;

public class AppLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, ILogger> _loggers;
    private readonly IConfiguration _configuration;

    public AppLoggerProvider(IConfiguration configuration)
    {
        _loggers = new();
        _configuration = configuration;
    }

    public ILogger CreateLogger(string categoryName)
    {
        ArgumentNullException.ThrowIfNull(categoryName, nameof(categoryName));

        if (_loggers.TryGetValue(categoryName, out ILogger? logger))
            return logger;

        logger = new AppLogger(categoryName, _configuration);
        _loggers.TryAdd(categoryName, logger);

        return logger;
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}
