using System;
using System.Collections.Generic;
using System.Text;
using EduPlay.DAL.Entities;

namespace EduPlay.DAL.Interfaces
{
    public interface IEduPlayDAL
    {
        public void AddUserGameRecords(UserGameRecords gameRecord);
        public void UpdateUserGameRecords(UserGameRecords gameRecord);
        public void RemoveUserGameRecords(UserGameRecords gameRecord);
        public List<UserGameRecords> GetUserGameRecordsByUserId(string userId);
        public List<UserGameRecords> GetUserGameRecordsByGameId(Guid gameId);
        public List<Games> GetAllGames();
        public void RemoveGame(Games game);
        public void UpdateGame(Games game);
        public Games GetGameById(Guid gameId);
        public List<Games> GetGamesByThemeId(Guid themeId);
        public List<Games> GetGamesByDifficultyId(Guid difficultyId);
        public List<AspNetUsers> GetAllUsers();
        public AspNetUsers GetUserById(string userId);
        public AspNetUsers GetUserByEmail(string email);
        public void UpdateUser(AspNetUsers user);
    }
}
