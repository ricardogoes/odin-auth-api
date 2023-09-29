using Microsoft.Extensions.Configuration;

namespace Odin.Auth.Domain.Models.AppSettings
{
    public class KeycloakSettings
    {
        [ConfigurationKeyName("realm")]
        public string? Realm { get; set; }
        
        [ConfigurationKeyName("auth-server-url")]
        public string? AuthServerUrl { get; set; }
        
        [ConfigurationKeyName("ssl-required")]
        public string? SslRequired { get; set; }
        
        [ConfigurationKeyName("resource")]
        public string? Resource { get; set; }
        
        [ConfigurationKeyName("verify-token-audience")]
        public bool VerifyTokenAudience { get; set; }
        
        [ConfigurationKeyName("credentials")]
        public KeycloakCredentials? Credentials { get; set; }
        
        [ConfigurationKeyName("confidential-port")]
        public int ConfidentialPort { get; set; }
        
        [ConfigurationKeyName("RolesSource")]
        public string? RolesSource { get; set; }
    }

    public class KeycloakCredentials
    {
        [ConfigurationKeyName("secret")]
        public string? Secret { get; set; }
        
        [ConfigurationKeyName("admin-username")]
        public string? AdminUsername { get; set; }
        
        [ConfigurationKeyName("admin-password")] 
        public string? AdminPassword { get; set; }
    }
}
