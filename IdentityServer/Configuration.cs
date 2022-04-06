using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource>()
            {
                new ApiResource()
                {
                    Name = "ApiOne",
                    Scopes = { "ScopeApiOne" }
                },
                new ApiResource()
                {
                    Name = "ApiTwo",
                    Scopes = { "ScopeApiTwo" }
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
                },
                new Client()
                {
                    ClientId = "client_id_mvc",
                    ClientSecrets = { new Secret("client_secret_mvc".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:7116/signin-oidc" },

                    AllowedScopes = { 
                        "ScopeApiOne", 
                        "ScopeApiTwo", 
                        IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServerConstants.StandardScopes.Profile 
                    }
                }
            };

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                // backward compat
                new ApiScope("ScopeApiOne"),
                new ApiScope("ScopeApiTwo")
            };
        }
    }
}
