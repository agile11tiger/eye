using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;
namespace EyEServer.Controllers.Identity;

public class OidcConfiguration(IClientRequestParametersProvider clientRequestParametersProvider) : ControllerBase
{
    public IClientRequestParametersProvider ClientRequestParametersProvider { get; } = clientRequestParametersProvider;

    [HttpGet("_configuration/{clientId}")]
    public IActionResult GetClientRequestParameters([FromRoute] string clientId)
    {
        var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
        return Ok(parameters);
    }
}
