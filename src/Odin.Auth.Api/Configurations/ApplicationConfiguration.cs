using FluentValidation;
using Odin.Auth.Application.CreateUser;

namespace Odin.Auth.Api.Configurations
{
    public static class ApplicationConfiguration
    {

        public static IServiceCollection AddKeycloakApplications(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateUser).Assembly);
            });

            ValidatorOptions.Global.LanguageManager.Enabled = false;

            services.AddValidatorsFromAssemblyContaining<CreateUserInputValidator>();

            return services;
        }
    }
}
