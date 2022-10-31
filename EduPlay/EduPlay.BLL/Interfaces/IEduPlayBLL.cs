using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EduPlay.BLL.Models;

namespace EduPlay.BLL.Interfaces
{
    public interface IEduPlayBLL
    {
        public Task AddUserGameRecord(UserGameRecordDTO gameRecordDTO);
        public Task UpdateUserGameRecord(UserGameRecordDTO newGameRecordDTO);
        public void RemoveGameRecordDTO(UserGameRecordDTO gameRecordDTO);
        public Task<List<UserGameRecordDTO>> GetUserGameRecordsByUserId(string id);
        public Task<List<UserGameRecordDTO>> GetUserGameRecordsByGameId(Guid id);
        public Task<List<GameDTO>> GetAllGames();
        public Task<List<GameDTO>> GetGamesByThemeId(Guid id);
        public Task<List<GameDTO>> GetGamesByDifficultyId(Guid id);
        public Task<GameDTO> GetGameById(Guid id);
        public List<UserDTO> GetAllUsers();
        public Task<UserDTO> GetUserById(string id);
        public UserDTO GetUserByEmail(string email);
        public Task UpdateUser(UserDTO user);
        public Task UpdateUserProfilePicture(string userId, string picture);
        public Task UpdateUserUserName(string userId, string userName);
        //public Task<UserDTO> GetUserByUserName(string userName);
    }
}
