namespace Odin.Auth.Domain.Models.UserProfile
{
    public class UserProfileResponse
    {
        public string Username { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PreferredUsername { get; set; }
    }
}
