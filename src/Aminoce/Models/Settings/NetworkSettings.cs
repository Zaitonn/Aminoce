using System;

namespace Aminoce.Models.Settings;

public class NetworkSettings
{
    public string[] UrlPrefixes { get; set; } = Array.Empty<string>();

    public bool AllowCrossOrigin { get; init; }

    public int MaxRequestsPerSecond { get; init; } = 50;

    public string[] WhiteList { get; init; } = Array.Empty<string>();

    public string[] AccessTokens { get; init; } = Array.Empty<string>();

    public CertificateSetting Certificate { get; init; } = new();
}