using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;

using Microsoft.Extensions.Hosting;

namespace Aminoce;

public static class CommandLineFactory
{
    public static Parser Create()
    {
        var rootCommnad = new RootCommand();
        var settingsCmd = new Command("setting", "Create new 'appsettings.json'");

        rootCommnad.AddCommand(settingsCmd);
        settingsCmd.SetHandler(CreateSettingFile);
        rootCommnad.SetHandler(() =>
        {
            if (!File.Exists("appsettings.json"))
                CreateSettingFile();

            new AppBuilder().Build().Run();
        });

        return new CommandLineBuilder(rootCommnad)
            .UseExceptionHandler(WriteError)
            .UseTypoCorrections()
            .UseVersionOption()
            .UseHelp()
            .UseTypoCorrections()
            .UseParseErrorReporting()
            .RegisterWithDotnetSuggest()
            .Build();
    }

    private static void CreateSettingFile()
    {
        using var filestream = new FileStream(
            "appsettings.json",
            FileMode.Create,
            FileAccess.Write
        );
        var stream =
            typeof(AppBuilder).Assembly.GetManifestResourceStream(
                "Aminoce.Sources.appsettings.json"
            )
            ?? throw new NotSupportedException(
                "Aminoce.Sources.appsettings.json is missing. Please check the building environment"
            );

        stream.CopyTo(filestream);
        filestream.Flush();
        filestream.Close();
    }

    private static void WriteError(Exception e, InvocationContext? context)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e.ToString());

        if (!Console.IsInputRedirected)
            Console.ReadLine();

        Console.ResetColor();

        if (e.HResult != 0 && context is not null)
            context.ExitCode = e.HResult;
    }
}