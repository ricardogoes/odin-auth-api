using Odin.Auth.Infra.Cognito.Models;

namespace Odin.Auth.Application.Common
{
    public class AppSettings
    {
        public CognitoSettings AWSCognitoSettings { get; private set; }

        public AppSettings(CognitoSettings awsCognitoSettings)
        {
            AWSCognitoSettings = awsCognitoSettings;
        }
    }
}
