using System.ComponentModel.DataAnnotations;

namespace EduPlay.WebAPI.Auth
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
