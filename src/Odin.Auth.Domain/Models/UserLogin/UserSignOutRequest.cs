using System.ComponentModel.DataAnnotations;

namespace Odin.Auth.Domain.Models.UserLogin
{
    public class UserSignOutRequest
    {
        [Required(ErrorMessage = "Username required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "AccessToken required")]
        public string AccessToken { get; set; }
    }
}
