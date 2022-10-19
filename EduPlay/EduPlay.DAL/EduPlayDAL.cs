using System;
using System.Collections.Generic;
using System.Text;
using EduPlay.DAL.Interfaces;
using EduPlay.DAL.Entities;
using System.Linq;

namespace EduPlay.DAL
{
    public class EduPlayDAL : IEduPlayDAL
    {
        private EduPlayContext _dbContext;

        public EduPlayDAL()
        {
            _dbContext = new EduPlayContext();
        }

        public void AddUserGameRecords(UserGameRecords gameRecord)
        {
            _dbContext.UserGameRecords.AddAsync(gameRecord);
            _dbContext.SaveChangesAsync();
        }

        public List<Games> GetAllGames()
        {
            return _dbContext.Games.ToList();
        }

        public List<AspNetUsers> GetAllUsers()
        {
            return _dbContext.AspNetUsers.ToList();
        }

        public Games GetGameById(Guid gameId)
        {
            return _dbContext.Games.Where(x => x.Id == gameId).FirstOrDefault();
        }

        public List<Games> GetGamesByDifficultyId(Guid difficultyId)
        {
            return _dbContext.Games.Where(x => x.DifficultyId == difficultyId).ToList();
        }

        public List<Games> GetGamesByThemeId(Guid themeId)
        {
            return _dbContext.Games.Where(x => x.ThemeId == themeId).ToList();
        }

        public AspNetUsers GetUserByEmail(string email)
        {
            return _dbContext.AspNetUsers.Where(x => x.Email == email).FirstOrDefault();
        }

        public AspNetUsers GetUserById(string userId)
        {
            return _dbContext.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();
        }

        public List<UserGameRecords> GetUserGameRecordsByGameId(Guid gameId)
        {
            return _dbContext.UserGameRecords.Where(x => x.GameId == gameId).ToList();
        }

        public List<UserGameRecords> GetUserGameRecordsByUserId(string userId)
        {
            return _dbContext.UserGameRecords.Where(x => x.UserId == userId).ToList();
        }

        public void RemoveGame(Games game)
        {
            _dbContext.Games.Remove(game);
            _dbContext.SaveChangesAsync();
        }

        public void RemoveUserGameRecords(UserGameRecords gameRecord)
        {
            _dbContext.UserGameRecords.Remove(gameRecord);
            _dbContext.SaveChangesAsync();
        }

        public void UpdateGame(Games game)
        {
            _dbContext.Games.Update(game);
            _dbContext.SaveChangesAsync();
        }

        public void UpdateUser(AspNetUsers user)
        {
            _dbContext.AspNetUsers.Update(user);
        }

        public void UpdateUserGameRecords(UserGameRecords gameRecord)
        {
            _dbContext.UserGameRecords.Update(gameRecord);
            _dbContext.SaveChangesAsync();
        }
    }
}
