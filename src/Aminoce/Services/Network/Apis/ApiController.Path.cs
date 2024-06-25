using System;
using System.IO;
using System.Threading.Tasks;

using Aminoce.Models.Network;
using Aminoce.Utils;

using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace Aminoce.Services.Network.Apis;

public partial class ApiController
{
    [Route(HttpVerbs.Get, "/path/exist")]
    public async Task Exist(
        [QueryField] string? path,
        [QueryField] string? file,
        [QueryField] string? dir
    )
    {
        if (!string.IsNullOrEmpty(file))
            await HttpContext.SendPacketAsync<bool>(new(File.Exists(file), DataType.Boolean));
        else if (!string.IsNullOrEmpty(dir))
            await HttpContext.SendPacketAsync<bool>(new(Directory.Exists(dir), DataType.Boolean));
        else
            await HttpContext.SendPacketAsync<bool>(new(Path.Exists(path), DataType.Boolean));
    }

    [Route(HttpVerbs.Get, "/path/readText")]
    public async Task ReadText(
        [QueryField(true)] string path,
        [QueryField] string? encoding = "utf8"
    )
    {
        await HttpContext.SendPacketAsync<string>(
            new(File.ReadAllText(path, EncodingFactory.GetEncoding(encoding)), DataType.String)
        );
    }

    [Route(HttpVerbs.Get, "/path/readBytes")]
    public async Task ReadBytes([QueryField(true)] string path)
    {
        await HttpContext.SendPacketAsync<string>(
            new(Convert.ToBase64String(File.ReadAllBytes(path)), DataType.String)
        );
    }

    [Route(HttpVerbs.Post, "/path/writeBytes")]
    public async Task WriteBytes([QueryField(true)] string path, [FormField(true)] string base64)
    {
        File.WriteAllBytes(path, Convert.FromBase64String(base64));
        await HttpContext.SendPacketAsync<string>(new(null, DataType.String));
    }
}
