using System.CommandLine.Parsing;

namespace Aminoce;

public static class Program
{
    public static int Main(string[] args)
    {
        Console.CancelKeyPress += (_, _) => Console.WriteLine("^C");

        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            Console.Title = $"Aminoce {AminoceApp.InformationalVersion}";

        return CommandLineHelper.Create().Invoke(args);
    }
}
