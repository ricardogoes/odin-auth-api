using Odin.Auth.Domain.ValuesObjects;
using Odin.Auth.Infra.Keycloak.Models;

namespace Odin.Auth.Infra.Keycloak.Mappers
{
    public static class CredentialMapper
    {
        public static CredentialRepresentation ToCredentialRepresentation(this UserCredential credential)
        {
            return new CredentialRepresentation
            {
                Type = credential.Type,
                Value = credential.Value,
                Temporary = credential.Temporary
            };
        }

        public static List<CredentialRepresentation> ToCredentialRepresentation(this List<UserCredential> credentials)
            => credentials.Select(ToCredentialRepresentation).ToList();

        public static UserCredential ToUserCredential(this CredentialRepresentation credential)
        {
            return new UserCredential
            (
                value: credential.Value!,
                temporary: credential.Temporary ?? false
            );
        }

        public static List<UserCredential> ToCredentialRepresentation(this List<CredentialRepresentation> credentials)
            => credentials.Select(ToUserCredential).ToList();
    }
}
