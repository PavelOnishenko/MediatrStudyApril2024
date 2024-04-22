using IdentityServer4.Models;

namespace PowerNetworkWebService;

public static class IdentityServerConfig
{
    public static IEnumerable<ApiScope> ApiScopes => [new ApiScope("writeAPI", "Write API")];

    public static IEnumerable<Client> Clients =>
       [
            new Client
            {
                ClientId = "apiClient", ClientSecrets = { new Secret("superSecretPassword".Sha256()) }, 
                AllowedGrantTypes = GrantTypes.ClientCredentials,AllowedScopes = { "writeAPI" } 
            }
       ];

    public static IEnumerable<IdentityResource> IdentityResources => [new IdentityResources.OpenId(), new IdentityResources.Profile()];
}
