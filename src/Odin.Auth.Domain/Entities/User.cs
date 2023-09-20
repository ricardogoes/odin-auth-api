using Odin.Auth.Domain.SeedWork;
using Odin.Auth.Domain.Validations;
using Odin.Auth.Domain.ValuesObjects;
using System.Text.Json.Serialization;

namespace Odin.Auth.Domain.Entities
{
    public class User : Entity
    {        
        public string Username { get; private set; }
        public bool Enabled { get; private set; }
        public bool EmailVerified { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public Dictionary<string, string> Attributes { get; private set; }
        public List<UserCredential> Credentials { get; private set; }
        public List<UserGroup> Groups { get; set; }

        [JsonConstructor]
        public User(Guid id, string username, string firstName, string lastName, string email, bool enabled) 
            : base(id)
        {
            Username = username;
            Enabled = enabled;
            EmailVerified = true;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Attributes = new Dictionary<string, string>();
            Credentials = new List<UserCredential>();
            Groups = new List<UserGroup>();

            Validate();
        }

        public User(string username, string firstName, string lastName, string email) 
            : base()
        {
            Username = username;
            Enabled = true;
            EmailVerified = true;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Attributes = new Dictionary<string, string>();
            Credentials = new List<UserCredential>();
            Groups = new List<UserGroup>();

            Validate();
        }

        public void Update(string? firstName, string? lastName, string? email)
        {
            FirstName = firstName ?? FirstName;
            LastName = lastName ?? LastName;
            Email = email ?? Email;

            Validate();
        }

        public void AddAttribute(KeyValuePair<string, string> attribute)
        {
            if (Attributes.ContainsKey(attribute.Key))
                Attributes.Remove(attribute.Key);

            Attributes.Add(attribute.Key, attribute.Value);

            Validate();
        }

        public void AddCredentials(UserCredential credential)
        {
            Credentials = new List<UserCredential> { credential };
            Validate();
        }

        public void RemoveAllGroups()
        {
            Groups = new List<UserGroup>();
            Validate();
        }

        public void AddGroup(UserGroup group)
        {
            Groups.Add(group);
            Validate();
        }

        public void Activate()
        {
            Enabled = true;
            Validate();
        }

        public void Deactivate()
        {
            Enabled = false;
            Validate();
        }

        private void Validate()
        {
            DomainValidation.NotNullOrEmpty(Username, nameof(Username));
            DomainValidation.NotNullOrEmpty(FirstName, nameof(FirstName));
            DomainValidation.NotNullOrEmpty(LastName, nameof(LastName));
            DomainValidation.Email(Email, nameof(Email));
        }
    }
}
