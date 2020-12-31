using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EyE.Server.Controllers.Identity
{
    public class OidcConfiguration : ControllerBase
    {
        private readonly ILogger<OidcConfiguration> logger;

        public OidcConfiguration(
            IClientRequestParametersProvider clientRequestParametersProvider,
            ILogger<OidcConfiguration> logger)
        {
            ClientRequestParametersProvider = clientRequestParametersProvider;
            this.logger = logger;
        }

        public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

        [HttpGet("_configuration/{clientId}")]
        public IActionResult GetClientRequestParameters([FromRoute] string clientId)
        {
            var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
            return Ok(parameters);
        }
    }
}
