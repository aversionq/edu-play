using System;
using System.Collections.Generic;
using System.Text;

namespace EduPlay.BLL.Models
{
    public class GameDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid ThemeId { get; set; }
        public int AgeLimit { get; set; }
        public int MaxScore { get; set; }
    }
}
