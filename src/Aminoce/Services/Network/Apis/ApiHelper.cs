using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

using Aminoce.Models.Network;

using EmbedIO;

namespace Aminoce.Services.Network.Apis;

public static class ApiHelper
{
    private static readonly Encoding UTF8 = new UTF8Encoding(false);

    private static readonly JsonSerializerOptions CamelCase =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

    public static async Task SendPacketAsync<T>(
        this IHttpContext httpContext,
        Packet<T> packet,
        HttpStatusCode httpStatusCode
    ) => await SendPacketAsync(httpContext, packet, (int)httpStatusCode);

    public static async Task SendPacketAsync<T>(
        this IHttpContext httpContext,
        Packet<T> packet,
        int httpStatusCode = 200
    )
    {
        httpContext.Response.StatusCode = httpStatusCode;
        await httpContext.SendStringAsync(
            JsonSerializer.Serialize(packet, CamelCase),
            "text/json",
            UTF8
        );
        httpContext.SetHandled();
    }
}