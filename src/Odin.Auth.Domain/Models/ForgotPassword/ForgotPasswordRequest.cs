using System.ComponentModel.DataAnnotations;

namespace Odin.Auth.Domain.Models.ForgotPassword
{
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage = "Username required")]
        public string Username { get; set; }
    }
}
