using System.Text.Json.Serialization;

namespace Aminoce.Models.Network;

[JsonConverter(typeof(JsonStringEnumConverter<DataType>))]
public enum DataType
{
    Unknown,

    Null,

    String,

    Number,

    Array,

    Object,

    Date,

    Base64,
}
