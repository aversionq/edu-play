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
        public Task UpdateUserGameRecords(Guid recordId, int newScore, int newTimesPlayed);
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
        public Task UpdateUserProfilePicture(string userId, string picture);
        public Task UpdateUserUserName(string userId, string userName);
        public Task<AspNetUsers> GetUserByUserName(string userName);
        public Task<List<Themes>> GetAllThemes();
        public Task<Themes> GetThemeById(Guid id);
        public Task<List<Difficulties>> GetAllDifficulties();
        public Task<Difficulties> GetDifficultyById(Guid id);
        public Task<Themes> GetThemeByName(string name);
        public Task<Difficulties> GetDifficultyByValue(int value);
        public Task UpdateTimesPlayed(Guid recordId, int amount);
        public Task<UserGameRecords> GetUserGameRecordsByUserIdAndGameId(string userId, Guid gameId);
    }
}
