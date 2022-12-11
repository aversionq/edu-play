using System;
using System.Collections.Generic;
using System.Text;

namespace EduPlay.BLL.Models
{
    public class UserGameRecordDTO
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public int Score { get; set; }
        public string UserId { get; set; }
        public int TimesPlayed { get; set; }
    }
}
