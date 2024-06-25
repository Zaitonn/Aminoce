using System;
using System.Text.Json.Serialization;

namespace Aminoce.Models.Network;

public class Packet<T>(T? data, DataType type)
{
    public string Type { get; init; } = type.ToString().ToLowerInvariant();

    [JsonPropertyName("_internalType")]
    public string? InternalType { get; } = type == DataType.Unknown ? null : typeof(T).ToString();

    public T? Data { get; } = data;

    public string? Message { get; set; }

    public DateTime Time { get; } = DateTime.Now;
}
