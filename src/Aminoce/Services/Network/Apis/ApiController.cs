using EmbedIO.WebApi;

using Microsoft.Extensions.Configuration;

namespace Aminoce.Services.Network.Apis;

public partial class ApiController(IConfiguration configuration) : WebApiController
{
    private readonly IConfiguration _configuration = configuration;
}