using IdentityModel;
using IdentityServer4.Models;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<ApiResource> GetApi() =>
            new List<ApiResource>()
            {
                new ApiResource()
                {
                    Name = "ApiOne",
                    Scopes = { "ScopeApiOne" }
                }
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>()
            {
                new Client()
                {
                    ClientId = "client_id",
                    ClientSecrets = { new Secret("client_secret".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    AllowedScopes = { "ScopeApiOne" } 
                }
            };

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                // backward compat
                new ApiScope("ScopeApiOne")
            };
        }
    }
}
