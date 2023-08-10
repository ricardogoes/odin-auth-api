using System.ComponentModel.DataAnnotations;

namespace Odin.Auth.Domain.Models.ChangePassword
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "EmailAddress required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Username { get; set; }

        [Required(ErrorMessage = "CurrentPassword required")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "NewPassword required")]
        public string NewPassword { get; set; }
    }
}
