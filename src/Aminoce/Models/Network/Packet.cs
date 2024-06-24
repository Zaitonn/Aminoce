namespace Aminoce.Models.Network;

public class Packet<T>(T? data, DataType type)
{
    public T? Data { get; } = data;

    public string InternalType { get; } = typeof(T).ToString();

    public string? Message { get; set; }

    public DataType Type { get; init; } = type;

    public DateTime Time { get; } = DateTime.Now;
}
