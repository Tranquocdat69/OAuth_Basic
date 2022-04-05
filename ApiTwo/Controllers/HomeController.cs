using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using static IdentityModel.OidcConstants;

namespace ApiTwo.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            HttpClient serverClient = _httpClientFactory.CreateClient();
            DiscoveryDocumentResponse discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:7104/");
            IdentityModel.Client.TokenResponse tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,
                
                ClientId = "client_id",
                ClientSecret = "client_secret",

                Scope = "ScopeApiOne"
            });

            HttpClient apiClient = _httpClientFactory.CreateClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            HttpResponseMessage httpResponseMessage = await apiClient.GetAsync("https://localhost:7000/Secret");
            string content = await httpResponseMessage.Content.ReadAsStringAsync();

            return Ok(new
            {
                access_token = tokenResponse.AccessToken,
                message = content
            });
        }
    }
}
