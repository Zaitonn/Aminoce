using System.Threading.Tasks;

using Aminoce.Models.Network;

using EmbedIO;
using EmbedIO.Routing;

namespace Aminoce.Services.Network.Apis;

public partial class ApiController
{
    [Route(HttpVerbs.Get, "/version")]
    public async Task GetVersion()
    {
        await HttpContext.SendPacketAsync<string>(new(App.Version.ToString(), DataType.String));
    }
}
