using Odin.Auth.Domain.Interfaces.Cognito;
using Odin.Auth.Service.Cognito;

namespace Odin.Auth.Api.IoC
{
    public class ServiceCognito : ServiceBase
    {
        protected override void HttpClient(IServiceCollection services)
        {
            base.HttpClient(services);
        }

        protected override void Scoped(IServiceCollection services)
        {

            services.AddScoped<IAmazonCognitoIdentityService, AmazonCognitoIdentityService>();
            base.Scoped(services);
        }
    }
}
