using Odin.Auth.Domain.Interfaces.Services;
using Odin.Auth.Service.ExternalServices;

namespace Odin.Auth.Api.IoC
{
    public class ServiceServices : ServiceBase
    {
        protected override void HttpClient(IServiceCollection services)
        {
            base.HttpClient(services);
        }

        protected override void Scoped(IServiceCollection services)
        {

            services.AddScoped<ICognitoAuthService, CognitoAuthService>();
            services.AddScoped<ICognitoUserService, CognitoUserService>();
            base.Scoped(services);
        }
    }
}
