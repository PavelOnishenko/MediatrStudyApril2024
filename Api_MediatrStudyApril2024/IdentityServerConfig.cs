using IdentityServer4.Models;

namespace Api_MediatrStudyApril2024
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("writeAPI", "Write API")
            };

        public static IEnumerable<Client> Clients =>
           new List<Client>
           {
                new Client
                {
                    ClientId = "apiClient",
                    ClientSecrets = { new Secret("superSecretPassword".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials, // Suitable for service-to-service communication
                    AllowedScopes = { "writeAPI" } // This client can request both read and write scopes
                }
           };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource> { new IdentityResources.OpenId(), new IdentityResources.Profile() };
    }
}
