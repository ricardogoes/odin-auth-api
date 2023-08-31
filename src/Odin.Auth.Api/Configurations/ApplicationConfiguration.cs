using Odin.Auth.Application.Common;
using Odin.Auth.Application.Login;

namespace Odin.Baseline.Api.Configurations
{
    public static class ApplicationConfiguration
    {

        public static IServiceCollection AddApplications(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Login).Assembly);
            });

            services.AddScoped<ICommonService, CommonService>();

            return services;
        }
    }
}
