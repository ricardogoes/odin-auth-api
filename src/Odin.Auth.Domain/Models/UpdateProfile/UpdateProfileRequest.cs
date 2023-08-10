using System.ComponentModel.DataAnnotations;

namespace Odin.Auth.Domain.Models.UpdateProfile
{
    public class UpdateProfileRequest
    {
        [Required(ErrorMessage = "Username required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "FirstName required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "EmailAddress required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string EmailAddress { get; set; }  
    }
}
