using System;
using System.Collections.Generic;
using System.Text;
using EduPlay.DAL.Interfaces;
using EduPlay.DAL.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EduPlay.DAL
{
    public class EduPlayDAL : IEduPlayDAL
    {
        private EduPlayContext _dbContext;

        public EduPlayDAL()
        {
            _dbContext = new EduPlayContext();
        }

        public async Task AddUserGameRecords(UserGameRecords gameRecord)
        {
            await _dbContext.UserGameRecords.AddAsync(gameRecord);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Games>> GetAllGames()
        {
            return await _dbContext.Games.ToListAsync();
        }

        public List<AspNetUsers> GetAllUsers()
        {
            return _dbContext.AspNetUsers.ToList();
        }

        public async Task<Games> GetGameById(Guid gameId)
        {
            return await _dbContext.Games.Where(x => x.Id == gameId).FirstOrDefaultAsync();
        }

        public async Task<List<Games>> GetGamesByDifficultyId(Guid difficultyId)
        {
            return await _dbContext.Games.Where(x => x.DifficultyId == difficultyId).ToListAsync();
        }

        public async Task<List<Games>> GetGamesByThemeId(Guid themeId)
        {
            return await _dbContext.Games.Where(x => x.ThemeId == themeId).ToListAsync();
        }

        public AspNetUsers GetUserByEmail(string email)
        {
            return _dbContext.AspNetUsers.Where(x => x.Email == email).FirstOrDefault();
        }

        public async Task<AspNetUsers> GetUserById(string userId)
        {
            return await _dbContext.AspNetUsers.Where(x => x.Id == userId).FirstOrDefaultAsync();
        }

        public async Task<List<UserGameRecords>> GetUserGameRecordsByGameId(Guid gameId)
        {
            return await _dbContext.UserGameRecords.Where(x => x.GameId == gameId).ToListAsync();
        }

        public async Task<List<UserGameRecords>> GetUserGameRecordsByUserId(string userId)
        {
            return await _dbContext.UserGameRecords.Where(x => x.UserId == userId).ToListAsync();
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

        public async Task UpdateUser(AspNetUsers user)
        {
            _dbContext.AspNetUsers.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserGameRecords(UserGameRecords gameRecord)
        {
            _dbContext.UserGameRecords.Update(gameRecord);
            await _dbContext.SaveChangesAsync();
        }
    }
}
