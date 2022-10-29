using Microsoft.AspNetCore.Identity;
using System;
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
        public DateTime DateOfBirth { get; set; }
        public bool IsBanned { get; set; } = false;
        public string ProfilePicture { get; set; }
    }
}
