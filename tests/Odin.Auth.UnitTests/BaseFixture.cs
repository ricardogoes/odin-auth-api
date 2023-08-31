using Bogus;
using Odin.Auth.Application.Common;
using Odin.Auth.Infra.Cognito.Models;

namespace Odin.Auth.UnitTests
{
    public abstract class BaseFixture
    {
        public AppSettings AppSettings { get; set; }
        public Faker Faker { get; set; }

        public BaseFixture()
        {
            AppSettings = new AppSettings
            {
                AWSCognitoSettings = new CognitoSettings
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

            Faker = new Faker("pt_BR");
        }

       /*protected override void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var appSettings = new AppSettings
            {
                AWSCognitoSettings = new CognitoSettings
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

            var cognitoSettings = appSettings.AWSCognitoSettings;

            services
                .AddSingleton(appSettings)
                .AddSingleton(cognitoSettings)
                .AddTransient<IAmazonCognitoIdentityRepository, AmazonCognitoIdentityRepositoryMock>();
        }

        protected override ValueTask DisposeAsyncCore()
            => new();

        protected override IEnumerable<TestAppSettings> GetTestAppSettings()
        {
            yield return new() { Filename = "appsettings.json", IsOptional = false };
        }*/
    }
}
