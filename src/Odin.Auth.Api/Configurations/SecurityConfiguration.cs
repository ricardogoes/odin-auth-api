using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;

namespace Odin.Auth.Api.Configurations
{
    public static class SecurityConfiguration
    {

        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            var keycloakAuthOptions = configuration.GetSection("Keycloak").Get<KeycloakAuthenticationOptions>()!;
            keycloakAuthOptions.Credentials.Secret = Environment.GetEnvironmentVariable("OdinSettings:Keycloak:Credentials:Secret")!;

            var keycloakProtectionClienteOptions = configuration.GetSection("Keycloak").Get<KeycloakProtectionClientOptions>()!;
            keycloakProtectionClienteOptions.Credentials.Secret = Environment.GetEnvironmentVariable("OdinSettings:Keycloak:Credentials:Secret")!;

            services.AddKeycloakAuthentication(keycloakAuthOptions);

            services
                .AddAuthorization()
                .AddKeycloakAuthorization(keycloakProtectionClienteOptions)
                .AddHeaderPropagation(o =>
                {
                    o.Headers.Add("Authorization");
                });

            return services;
        }
    }
}
