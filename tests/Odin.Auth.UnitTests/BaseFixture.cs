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
            var cognitoSettings = new CognitoSettings
            (
                Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__AccessKeyId"),
                Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__AccessSecretKey"),
                Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__AppClientId"),
                Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__CognitoAuthorityUrl"),
                Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__CognitoIdpUrl"),
                Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__Region"),
                Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__UserPoolId")
            );

            AppSettings = new AppSettings(cognitoSettings);

            Faker = new Faker("pt_BR");
        }
    }
}
