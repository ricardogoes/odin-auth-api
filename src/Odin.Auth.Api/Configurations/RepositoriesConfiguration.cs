using Odin.Auth.Infra.Cognito;

namespace Odin.Auth.Api.Configurations
{
    public static class RepositoriesConfiguration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IAmazonCognitoIdentityRepository, AmazonCognitoIdentityRepository>();

            return services;
        }
    }
}
