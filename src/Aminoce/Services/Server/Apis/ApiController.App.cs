using Aminoce.Models.Network;

using EmbedIO;
using EmbedIO.Routing;

namespace Aminoce.Services.Server.Apis;

public partial class ApiController
{
    [Route(HttpVerbs.Get, "/")]
    public async Task Root()
    {
        await HttpContext.SendPacketAsync<string>(new("Hello world.", DataType.String));
    }

    [Route(HttpVerbs.Get, "/version")]
    public async Task GetVersion()
    {
        await HttpContext.SendPacketAsync<string>(
            new(AminoceApp.Version.ToString(), DataType.String)
        );
    }
}
