using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using RpgCalendar.Tools;

namespace RpgCalendar.API.Controllers;

[ApiController, Route("info")]
public class InfoController : Controller
{
    [HttpGet("keycloak")]
    public IActionResult Index()
    {
        HttpClient? httpKeycloakClient = null;
        FeatureFlag.RequireFeatureFlag(FeatureFlag.FeatureFlagEnum.KEYCLOAK_CERT, () =>
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
            httpKeycloakClient = new HttpClient(handler);
        });
        httpKeycloakClient ??= new HttpClient();

        var response = httpKeycloakClient.Send(new HttpRequestMessage(HttpMethod.Get, EnvironmentData.KeycloakMetadataUrl));
        return Ok(new{Status = response.StatusCode, Content = response.Content.ReadAsStringAsync().Result});
    }
}