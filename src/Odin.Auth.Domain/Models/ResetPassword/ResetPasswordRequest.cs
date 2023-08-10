using System.ComponentModel.DataAnnotations;

namespace Odin.Auth.Domain.Models.ResetPassword
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "UserId required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "NewPassword required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "ConfirmationCode required")]
        public string ConfirmationCode { get; set; }

        [Required(ErrorMessage = "Username required")]
        public string Username { get; set; }
    }
}
