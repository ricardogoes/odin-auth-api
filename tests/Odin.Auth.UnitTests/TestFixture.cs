using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odin.Auth.Domain.Interfaces.Cognito;
using Odin.Auth.Domain.Interfaces.Services;
using Odin.Auth.Domain.Models;
using Odin.Auth.Service.Cognito;
using Odin.Auth.Service.ExternalServices;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Odin.Auth.UnitTests
{
    public class TestFixture : TestBedFixture
    {
        protected override void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var appSettings = builder.Get<AppSettings>();

            services
                .AddSingleton(appSettings)
                .AddTransient<ICognitoAuthService, CognitoAuthService>()
                .AddTransient<ICognitoUserService, CognitoUserService>()
                .AddTransient<IAmazonCognitoIdentityService, AmazonCognitoIdentityServiceMock>();
        }

        protected override ValueTask DisposeAsyncCore()
            => new();

        protected override IEnumerable<TestAppSettings> GetTestAppSettings()
        {
            yield return new() { Filename = "appsettings.json", IsOptional = false };
        }
    }
}
