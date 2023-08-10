using System.ComponentModel.DataAnnotations;

namespace Odin.Auth.Domain.Models.UserLogin
{
    public class UserAuthRequest
    {
        [Required(ErrorMessage = "Username required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
    }
}
