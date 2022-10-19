using System;
using System.Collections.Generic;
using System.Text;
using EduPlay.BLL.Models;

namespace EduPlay.BLL.Interfaces
{
    public interface IEduPlayBLL
    {
        public void AddUserGameRecord(UserGameRecordDTO gameRecordDTO);
        public void UpdateUserGameRecord(UserGameRecordDTO newGameRecordDTO);
        public void RemoveGameRecordDTO(UserGameRecordDTO gameRecordDTO);
        public List<UserGameRecordDTO> GetUserGameRecordsByUserId(string id);
        public List<UserGameRecordDTO> GetUserGameRecordsByGameId(Guid id);
        public List<GameDTO> GetAllGames();
        public List<GameDTO> GetGamesByThemeId(Guid id);
        public List<GameDTO> GetGamesByDifficultyId(Guid id);
        public GameDTO GetGameById(Guid id);
        public List<UserDTO> GetAllUsers();
        public UserDTO GetUserById(string id);
        public UserDTO GetUserByEmail(string email);
        public void UpdateUser(UserDTO user);
    }
}
