using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

using Aminoce.Models.Network;

using EmbedIO;

namespace Aminoce.Services.Server.Apis;

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
    ) => await SendPacketAsync<T>(httpContext, packet, (int)httpStatusCode);

    public static async Task SendPacketAsync<T>(
        this IHttpContext httpContext,
        Packet<T> packet,
        int httpStatusCode = 200
    )
    {
        await httpContext.SendStringAsync(
            JsonSerializer.Serialize(packet, CamelCase),
            "text/json",
            UTF8
        );
        httpContext.Response.StatusCode = httpStatusCode;
        httpContext.SetHandled();
    }

    public static async Task OnHttpException(IHttpContext context, IHttpException httpException)
    {
        await context.SendPacketAsync<object>(
            new(null, DataType.Unknown) { Message = httpException.Message },
            httpException.StatusCode
        );
    }

    public static async Task OnException(IHttpContext context, Exception exception)
    {
        await context.SendPacketAsync<object>(
            new(null, DataType.Unknown) { Message = $"{exception.GetType()}:{exception.Message}" },
            HttpStatusCode.InternalServerError
        );
    }
}
