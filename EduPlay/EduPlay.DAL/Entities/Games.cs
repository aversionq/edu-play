﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EduPlay.WebAPI.Models
{
    public partial class Games
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid ThemeId { get; set; }

        public virtual Difficulties Difficulty { get; set; }
        public virtual Themes Theme { get; set; }
    }
}
