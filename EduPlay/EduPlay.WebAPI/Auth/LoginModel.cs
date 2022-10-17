﻿using System.ComponentModel.DataAnnotations;

namespace EduPlay.WebAPI.Auth
{
    public class LoginModel
    {
        [Required(ErrorMessage = "UserName or Email is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
