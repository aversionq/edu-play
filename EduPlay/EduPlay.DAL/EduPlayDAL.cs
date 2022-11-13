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
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task AddUserGameRecords(UserGameRecords gameRecord)
        {
            await _dbContext.UserGameRecords.AddAsync(gameRecord);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Difficulties>> GetAllDifficulties()
        {
            return await _dbContext.Difficulties.ToListAsync();
        }

        public async Task<List<Games>> GetAllGames()
        {
            return await _dbContext.Games.ToListAsync();
        }

        public async Task<List<Themes>> GetAllThemes()
        {
            return await _dbContext.Themes.ToListAsync();
        }

        public List<AspNetUsers> GetAllUsers()
        {
            return _dbContext.AspNetUsers.ToList();
        }

        public async Task<Difficulties> GetDifficultyById(Guid id)
        {
            return await _dbContext.Difficulties.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Difficulties> GetDifficultyByValue(int value)
        {
            return await _dbContext.Difficulties.Where(x => x.Value == value).FirstOrDefaultAsync();
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

        public async Task<Themes> GetThemeById(Guid id)
        {
            return await _dbContext.Themes.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Themes> GetThemeByName(string name)
        {
            return await _dbContext.Themes.Where(x => x.Name.ToUpper() == name.ToUpper())
                .FirstOrDefaultAsync();
        }

        public AspNetUsers GetUserByEmail(string email)
        {
            return _dbContext.AspNetUsers.Where(x => x.Email == email).FirstOrDefault();
        }

        public async Task<AspNetUsers> GetUserById(string userId)
        {
            return await _dbContext.AspNetUsers.Where(x => x.Id == userId).FirstOrDefaultAsync();
        }

        public async Task<AspNetUsers> GetUserByUserName(string userName)
        {
            return await _dbContext.AspNetUsers.Where(x => x.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<List<UserGameRecords>> GetUserGameRecordsByGameId(Guid gameId)
        {
            return await _dbContext.UserGameRecords.Where(x => x.GameId == gameId).ToListAsync();
        }

        public async Task<List<UserGameRecords>> GetUserGameRecordsByUserId(string userId)
        {
            return await _dbContext.UserGameRecords.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<UserGameRecords> GetUserGameRecordsByUserIdAndGameId(string userId, Guid gameId)
        {
            return await _dbContext.UserGameRecords.Where(
                x => x.UserId == userId && x.GameId == gameId).FirstOrDefaultAsync();
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

        public async Task UpdateTimesPlayed(Guid recordId, int amount)
        {
            var record = new UserGameRecords
            {
                Id = recordId,
                TimesPlayed = amount
            };
            _dbContext.UserGameRecords.Attach(record).Property(x => x.TimesPlayed).IsModified = true;
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(record).State = EntityState.Detached;
        }

        //public async Task UpdateTimesPlayed(string userId, Guid gameId, int amount)
        //{
        //    var record = new UserGameRecords
        //    {
        //        UserId = userId,
        //        GameId = gameId,
        //        TimesPlayed = amount
        //    };
        //    _dbContext.UserGameRecords.Attach(record).Property(x => x.TimesPlayed).IsModified = true;
        //    await _dbContext.SaveChangesAsync();
        //}

        public async Task UpdateUser(AspNetUsers user)
        {
            _dbContext.AspNetUsers.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserGameRecords(Guid recordId, int newScore, int newTimesPlayed)
        {
            var record = new UserGameRecords
            {
                Id = recordId,
                Score = newScore,
                TimesPlayed = newTimesPlayed
            };
            _dbContext.UserGameRecords.Attach(record);
            _dbContext.Entry(record).Property(x => x.Score).IsModified = true;
            _dbContext.Entry(record).Property(x => x.TimesPlayed).IsModified = true;
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(record).State = EntityState.Detached;
        }

        public async Task UpdateUserProfilePicture(string userId, string picture)
        {
            var user = new AspNetUsers { Id = userId, ProfilePicture = picture };
            _dbContext.AspNetUsers.Attach(user).Property(x => x.ProfilePicture).IsModified = true;
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(user).State = EntityState.Detached;
        }

        public async Task UpdateUserUserName(string userId, string userName)
        {
            var user = new AspNetUsers 
            { 
                Id = userId, 
                UserName = userName,
                NormalizedUserName = userName.ToUpper()
            };

            _dbContext.AspNetUsers.Attach(user);
            _dbContext.Entry(user).Property(x => x.UserName).IsModified = true;
            _dbContext.Entry(user).Property(x => x.NormalizedUserName).IsModified = true;
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(user).State = EntityState.Detached;
        }
    }
}
