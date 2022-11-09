using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EduPlay.WebAPI.Models
{
    public partial class Themes
    {
        public Themes()
        {
            Games = new HashSet<Games>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Games> Games { get; set; }
    }
}
