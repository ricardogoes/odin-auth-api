using System.ComponentModel.DataAnnotations;

namespace Odin.Auth.Domain.Models.UserLogin
{
    public class InsertUserRequest
    {
        [Required(ErrorMessage = "First Name required")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Last Name required")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Email Address required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string EmailAddress { get; set; }
        
        [Required(ErrorMessage = "Username required")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
    }
}
