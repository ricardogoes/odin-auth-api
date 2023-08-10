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
                .AddEnvironmentVariables()
                .Build();

            var appSettings = new AppSettings
            {
                AWSCognitoSettings = new AWSCognitoSettings
                {
                    AccessKeyId = Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__AccessKeyId"),
                    AccessSecretKey = Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__AccessSecretKey"),
                    AppClientId = Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__AppClientId"),
                    CognitoAuthorityUrl = Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__CognitoAuthorityUrl"),
                    CognitoIdpUrl = Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__CognitoIdpUrl"),
                    Region = Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__Region"),
                    UserPoolId = Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__UserPoolId")
                }
            };

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
