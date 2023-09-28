using FluentValidation;
using Odin.Auth.Application.Users.CreateUser;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Domain.Services;

namespace Odin.Auth.Api.Configurations
{
    public static class ApplicationConfiguration
    {

        public static IServiceCollection AddApplications(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateUser).Assembly);
            });

            ValidatorOptions.Global.LanguageManager.Enabled = false;

            services.AddValidatorsFromAssemblyContaining<CreateUserInputValidator>();

            services.AddScoped<IDocumentService, DocumentService>();

            return services;
        }
    }
}
