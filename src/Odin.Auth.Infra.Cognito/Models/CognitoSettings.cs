namespace Odin.Auth.Infra.Cognito.Models
{
    public class CognitoSettings
    {
        public string? CognitoIdpUrl { get; private set; }
        public string? CognitoAuthorityUrl { get; private set; }
        public string? Region { get; private set; }
        public string? UserPoolId { get; private set; }
        public string? AppClientId { get; private set; }
        public string? AccessKeyId { get; private set; }
        public string? AccessSecretKey { get; private set; }

        public CognitoSettings(string? cognitoIdpUrl, string? cognitoAuthorityUrl, string? region, string? userPoolId, string? appClientId, string? accessKeyId, string? accessSecretKey)
        {
            CognitoIdpUrl = cognitoIdpUrl;
            CognitoAuthorityUrl = cognitoAuthorityUrl;
            Region = region;
            UserPoolId = userPoolId;
            AppClientId = appClientId;
            AccessKeyId = accessKeyId;
            AccessSecretKey = accessSecretKey;
        }
    }
}
