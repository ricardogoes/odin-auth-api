using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Infra.Data.EF.Interfaces;
using Odin.Auth.Infra.Data.EF.Repositories;
using Odin.Auth.Infra.Keycloak.Interfaces;
using Odin.Auth.Infra.Keycloak.Repositories;

namespace Odin.Auth.Api.Configurations
{
    public static class RepositoryConfiguration
    {

        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>(); 
            services.AddScoped<ICustomerRepository, CustomerRepository>();            

            services.AddScoped<IAuthKeycloakRepository, AuthKeycloakRepository>();
            services.AddScoped<IUserKeycloakRepository, UserKeycloakRepository>();

            return services;
        }
    }
}
