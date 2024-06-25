using System.Text.Json.Serialization;

namespace Aminoce.Models.Network;

[JsonConverter(typeof(JsonStringEnumConverter<DataType>))]
public enum DataType
{
    Unknown,

    Null,

    String,

    Boolean,

    Number,

    Array,

    Object,

    Date,

    Base64,
}