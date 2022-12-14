using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EduPlay.DAL.Entities
{
    public partial class UserGameRecords
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public int Score { get; set; }
        public string UserId { get; set; }
        public int TimesPlayed { get; set; }

        public virtual Games Game { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
