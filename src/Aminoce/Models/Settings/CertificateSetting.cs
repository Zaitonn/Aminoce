namespace Aminoce.Models.Settings;

public class CertificateSetting
{
    public bool Enable { get; init; }

    public bool AutoRegisterCertificate { get; init; }

    public bool AutoLoadCertificate { get; init; }

    public string? Path { get; init; }

    public string? Password { get; init; }
}