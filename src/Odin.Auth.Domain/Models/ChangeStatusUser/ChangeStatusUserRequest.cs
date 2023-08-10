using System.ComponentModel.DataAnnotations;

namespace Odin.Auth.Domain.Models.ChangeStatusUser
{
    public class ChangeStatusUserRequest
    {
        [Required(ErrorMessage = "Username required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Action required")]
        [RegularExpression("ENABLE|DISABLE", ErrorMessage = "Invalid action. Only ENABLE or DISABLE values are allowed")]
        public string Action { get; set; }
    }
}
