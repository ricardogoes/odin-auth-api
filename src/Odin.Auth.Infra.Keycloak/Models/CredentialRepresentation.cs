using System.Text.Json.Serialization;

namespace Odin.Auth.Infra.Keycloak.Models
{
    public class CredentialRepresentation
    {
        [JsonPropertyName("id")]
        public Guid? Id { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("temporary")]
        public bool? Temporary { get; set; }
    }
}
