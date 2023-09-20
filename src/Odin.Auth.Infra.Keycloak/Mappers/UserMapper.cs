using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.ValuesObjects;
using Odin.Auth.Infra.Keycloak.Models;

namespace Odin.Auth.Infra.Keycloak.Mappers
{
    public static class UserMapper
    {
        public static UserRepresentation ToUserRepresentation(this User user)
        {
            var attributes = new Dictionary<string, List<string>>();
            
            foreach (var attr in user.Attributes)
            {
                attributes.Add(attr.Key, new List<string> { attr.Value });
            }

            return new UserRepresentation
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailVerified = user.EmailVerified,
                Enabled = user.Enabled,
                Attributes = attributes,
                Groups = user.Groups.Select(group => group.Name).ToList(),
                Credentials = user.Credentials!.ToCredentialRepresentation()                
            };
        }

        public static User ToUser(this UserRepresentation userRepresentation, List<UserGroup>? userGroups = null)
        {
            var user = new User
            (
                id: userRepresentation.Id!.Value,
                username: userRepresentation.Username!,
                firstName: userRepresentation.FirstName!,
                lastName: userRepresentation.LastName!,
                email: userRepresentation.Email!,
                enabled: userRepresentation.Enabled!.Value                
            );

            if (userRepresentation.Attributes is not null && userRepresentation.Attributes.Any())
            {
                foreach (var attribute in userRepresentation.Attributes)
                {
                    user.AddAttribute(new KeyValuePair<string, string>(attribute.Key, attribute.Value.First()));
                }
            }

            if (userGroups is not null && userGroups.Any())
            { 
                foreach (var group in userGroups)
                    user.AddGroup(group);
            }

            return user;
        }
    }
}
