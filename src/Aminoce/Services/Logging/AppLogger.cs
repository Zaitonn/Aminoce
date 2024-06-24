using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Spectre.Console;

namespace Aminoce.Services.Logging;

public class AppLogger(string title, IConfiguration configuration) : ILogger
{
    private readonly string _title = title;
    private readonly IConfiguration _configuration = configuration;

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _configuration.GetValue<LogLevel>("Logging:LogLevel:Default") > logLevel;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        var line = formatter(state, exception);
        switch (logLevel)
        {
            case LogLevel.Trace:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [mediumpurple4]Trace[/] {_title} {line}"
                );
                break;
            case LogLevel.Debug:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [mediumpurple4]Debug[/] {_title} {line}"
                );
                break;
            case LogLevel.Information:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [cadetblue_1]Information[/] {_title} {line}"
                );
                break;
            case LogLevel.Warning:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [yellow bold]Warning {_title} {line}[/]"
                );
                break;
            case LogLevel.Error:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [red bold]Error {_title} {line}[/]"
                );
                break;
            case LogLevel.Critical:
                AnsiConsole.MarkupLineInterpolated(
                    $"{DateTime.Now:T} [maroon blod]Critical {_title} {line}[/]"
                );
                break;
            case LogLevel.None:
                break;
        }
    }
}
