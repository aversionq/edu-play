using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EduPlay.DAL.Entities;

namespace EduPlay.DAL.Interfaces
{
    public interface IEduPlayDAL
    {
        public Task AddUserGameRecords(UserGameRecords gameRecord);
        public Task UpdateUserGameRecords(UserGameRecords gameRecord);
        public void RemoveUserGameRecords(UserGameRecords gameRecord);
        public Task<List<UserGameRecords>> GetUserGameRecordsByUserId(string userId);
        public Task<List<UserGameRecords>> GetUserGameRecordsByGameId(Guid gameId);
        public Task<List<Games>> GetAllGames();
        public void RemoveGame(Games game);
        public void UpdateGame(Games game);
        public Task<Games> GetGameById(Guid gameId);
        public Task<List<Games>> GetGamesByThemeId(Guid themeId);
        public Task<List<Games>> GetGamesByDifficultyId(Guid difficultyId);
        public List<AspNetUsers> GetAllUsers();
        public Task<AspNetUsers> GetUserById(string userId);
        public AspNetUsers GetUserByEmail(string email);
        public Task UpdateUser(AspNetUsers user);
    }
}
