using Keycloak.AuthServices.Authentication;
using Microsoft.Net.Http.Headers;
using Polly;

namespace Odin.Auth.Api.Configurations
{
    public static class HttpClientConfiguration
    {

        public static IServiceCollection AddHttpClientConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            var keycloakOptions = configuration
                .GetSection(KeycloakAuthenticationOptions.Section)
                .Get<KeycloakAuthenticationOptions>()!;

            services
                .AddHttpClient("Keycloak", httpClient =>
                {
                    httpClient.BaseAddress = new Uri(keycloakOptions.AuthServerUrl);
                })
                .AddHeaderPropagation()
                .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.RetryAsync(3))
                .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))); ;

            return services;
        }
    }
}
