namespace Odin.Auth.Domain.Models
{
    public class AppSettings
    {
        public AWSCognitoSettings AWSCognitoSettings { get; set; }
    }

    public class AWSCognitoSettings
    {
        public string CognitoIdpUrl { get; set; }
        public string CognitoAuthorityUrl { get; set; }
        public string Region { get; set; }
        public string UserPoolId { get; set; }
        public string AppClientId { get; set; }
        public string AccessKeyId { get; set; }
        public string AccessSecretKey { get; set; }
    }
}
