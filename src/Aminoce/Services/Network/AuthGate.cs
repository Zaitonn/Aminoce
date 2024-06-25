using System.Linq;
using System.Threading.Tasks;

using Aminoce.Models.Settings;

using EmbedIO;

using Microsoft.Extensions.Options;

namespace Aminoce.Services.Network;

public class AuthGate(IOptions<NetworkSettings> options) : WebModuleBase("/")
{
    private readonly IOptions<NetworkSettings> _options = options;

    public override bool IsFinalHandler => false;

    protected override Task OnRequestAsync(IHttpContext context)
    {
        if (!context.RequestedPath.StartsWith("/v1") || _options.Value.AccessTokens.Length == 0)
            return Task.CompletedTask;

        var auth = context.Request.Headers.Get("Authorization");

        if (string.IsNullOrEmpty(auth))
            throw HttpException.Unauthorized();

        if (auth!.StartsWith("Bearer "))
            auth = auth[7..];

        return !_options.Value.AccessTokens.Contains(auth)
            ? throw HttpException.Unauthorized()
            : Task.CompletedTask;
    }
}