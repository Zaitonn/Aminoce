using EmbedIO.WebApi;

using Microsoft.Extensions.Configuration;

namespace Aminoce.Services.Server.Apis;

public partial class ApiController(IConfiguration configuration) : WebApiController
{
    private readonly IConfiguration _configuration = configuration;
}
