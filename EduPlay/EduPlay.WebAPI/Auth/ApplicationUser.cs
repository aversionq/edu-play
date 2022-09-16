using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EduPlay.WebAPI.Auth
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public int Age { get; set; }
        public bool IsBanned { get; set; } = false;
    }
}
