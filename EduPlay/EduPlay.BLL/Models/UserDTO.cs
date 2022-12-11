using System;
using System.Collections.Generic;
using System.Text;

namespace EduPlay.BLL.Models
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsBanned { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ProfilePicture { get; set; }
    }
}
