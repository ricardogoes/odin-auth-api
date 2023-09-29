using System.Text.Json.Serialization;

namespace Odin.Auth.Infra.Keycloak.Models
{
    public class UserRepresentation
    {
        [JsonPropertyName("id")]
        public Guid? Id { get; set; }
        
        [JsonPropertyName("username")]
        public string? Username { get; set; }
        
        [JsonPropertyName("enabled")]
        public bool? Enabled { get; set; }
        
        [JsonPropertyName("emailVerified")]
        public bool? EmailVerified { get; set; }
        
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }
        
        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }
        
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        
        [JsonPropertyName("attributes")]
        public Dictionary<string, List<string>>? Attributes { get; set; }
        
        [JsonPropertyName("credentials")]
        public List<CredentialRepresentation>? Credentials { get; set; }
        
        [JsonPropertyName("groups")] 
        public List<string>? Groups { get; set; }
    }
}
