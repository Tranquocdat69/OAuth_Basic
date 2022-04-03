using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace Api.AuthorizationRequirement
{
    public class JwtRequirement : IAuthorizationRequirement
    {

    }

    public class JwtRequirementHandler : AuthorizationHandler<JwtRequirement>
    {
        private readonly HttpClient _httpClient;
        private readonly HttpContext _httpContext;

        public JwtRequirementHandler(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpContext = httpContextAccessor.HttpContext;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtRequirement requirement)
        {
            if (_httpContext.Request.Headers.TryGetValue("Authorization", out var auth))
            {
                string accessToken = auth.ToString().Split(' ')[1];
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"https://localhost:7151/OAuth/ValidateToken?access_token={accessToken}");
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
