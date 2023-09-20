using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Infra.Keycloak.Repositories;

namespace Odin.Auth.Api.Configurations
{
    public static class RepositoryConfiguration
    {

        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IKeycloakRepository, KeycloakRepository>();

            return services;
        }
    }
}
