using System;
using System.Text;

namespace Aminoce.Utils;

public static class EncodingFactory
{
    static EncodingFactory()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        UTF8 = new UTF8Encoding(false);
        LittleEndianUnicode = new UnicodeEncoding(false, false);
        BigEndianUnicode = new UnicodeEncoding(true, false);
        GBK = Encoding.GetEncoding("gbk");
    }

    public static readonly Encoding UTF8;
    public static readonly Encoding LittleEndianUnicode;
    public static readonly Encoding BigEndianUnicode;
    public static readonly Encoding GBK;

    public static Encoding GetEncoding(string? encodingType)
    {
        return encodingType?.ToUpperInvariant() switch
        {
           "UTF-8" => UTF8,
           "UTF8" => UTF8,
           "UTF16LE" => LittleEndianUnicode,
           "UTF16BE" => BigEndianUnicode,
           "GBK" => GBK,
           "GB2312" => GBK,
            _ => throw new ArgumentOutOfRangeException(nameof(encodingType))
        };
    }

}
