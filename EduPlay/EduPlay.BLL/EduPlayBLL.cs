using System;
using System.Collections.Generic;
using System.Text;
using EduPlay.DAL.Interfaces;
using EduPlay.DAL.Entities;
using EduPlay.BLL.Models;
using EduPlay.BLL.Interfaces;
using AutoMapper;
using System.Threading.Tasks;
using System.Linq;

namespace EduPlay.BLL
{
    public class EduPlayBLL : IEduPlayBLL
    {
        private IEduPlayDAL _dal;
        private Mapper _userMapper;
        private Mapper _gameMapper;
        private Mapper _userGameRecordMapper;
        private Mapper _themeMapper;
        private Mapper _difficultyMapper;

        public EduPlayBLL(IEduPlayDAL dal)
        {
            _dal = dal;
            SetupMappers();
        }

        public async Task<List<DifficultyDTO>> GetAllDifficulties()
        {
            var difficulties = await _dal.GetAllDifficulties();
            var difficultiesDTO = _difficultyMapper.Map<List<Difficulties>, List<DifficultyDTO>>(difficulties);
            return difficultiesDTO;
        }

        public async Task<List<GameDTO>> GetAllGames()
        {
            var games = await _dal.GetAllGames();
            List<GameDTO> gamesDto = _gameMapper.Map<List<Games>, List<GameDTO>>(games);
            return gamesDto;
        }

        public async Task<List<ThemeDTO>> GetAllThemes()
        {
            var themes = await _dal.GetAllThemes();
            var themesDTO = _themeMapper.Map<List<Themes>, List<ThemeDTO>>(themes);
            return themesDTO;
        }

        public List<UserDTO> GetAllUsers()
        {
            var users = _dal.GetAllUsers();
            var usersDto = _userMapper.Map<List<AspNetUsers>, List<UserDTO>>(users);
            return usersDto;
        }

        public async Task<DifficultyDTO> GetDifficultyById(Guid id)
        {
            var difficulty = await _dal.GetDifficultyById(id);
            var difficultyDTO = _difficultyMapper.Map<Difficulties, DifficultyDTO>(difficulty);
            return difficultyDTO;
        }

        public async Task<DifficultyDTO> GetDifficultyByValue(int value)
        {
            var difficulty = await _dal.GetDifficultyByValue(value);
            var difficultyDTO = _difficultyMapper.Map<Difficulties, DifficultyDTO>(difficulty);
            return difficultyDTO;
        }

        public async Task<GameDTO> GetGameById(Guid id)
        {
            var game = await _dal.GetGameById(id);
            var gameDto = _gameMapper.Map<Games, GameDTO>(game);
            return gameDto;
        }

        public async Task<List<GameDTO>> GetGamesByDifficultyId(Guid id)
        {
            var games = await _dal.GetGamesByDifficultyId(id);
            var gamesDto = _gameMapper.Map<List<Games>, List<GameDTO>>(games);
            return gamesDto;
        }

        public async Task<List<GameDTO>> GetGamesByThemeId(Guid id)
        {
            var games = await _dal.GetGamesByThemeId(id);
            var gamesDto = _gameMapper.Map<List<Games>, List<GameDTO>>(games);
            return gamesDto;
        }

        public async Task<ThemeDTO> GetThemeById(Guid id)
        {
            var theme = await _dal.GetThemeById(id);
            var themeDTO = _themeMapper.Map<Themes, ThemeDTO>(theme);
            return themeDTO;
        }

        public async Task<ThemeDTO> GetThemeByName(string name)
        {
            var theme = await _dal.GetThemeByName(name);
            var themeDTO = _themeMapper.Map<Themes, ThemeDTO>(theme);
            return themeDTO;
        }

        public async Task<GameDTO> GetUserBestResult(string userId)
        {
            var userRecords = await _dal.GetUserGameRecordsByUserId(userId);
            var bestResult = userRecords.OrderByDescending(x => x.Score).FirstOrDefault();
            var game = await _dal.GetGameById(bestResult.GameId);
            var gameDTO = _gameMapper.Map<Games, GameDTO>(game);
            return gameDTO;
        }

        public UserDTO GetUserByEmail(string email)
        {
            var user = _dal.GetUserByEmail(email);
            var userDto = _userMapper.Map<AspNetUsers, UserDTO>(user);
            return userDto;
        }

        public async Task<UserDTO> GetUserById(string id)
        {
            var user = await _dal.GetUserById(id);
            var userDto = _userMapper.Map<AspNetUsers, UserDTO>(user);
            return userDto;
        }

        public async Task<List<UserGameRecordDTO>> GetUserGameRecordsByGameId(Guid id)
        {
            var records = await _dal.GetUserGameRecordsByGameId(id);
            var recordsDto = _userGameRecordMapper.Map<List<UserGameRecords>, List<UserGameRecordDTO>>(records);
            return recordsDto;
        }

        public async Task<List<UserGameRecordDTO>> GetUserGameRecordsByUserId(string id)
        {
            var users = await _dal.GetUserGameRecordsByUserId(id);
            var usersDto = _userGameRecordMapper.Map<List<UserGameRecords>, List<UserGameRecordDTO>>(users);
            return usersDto;
        }

        public async Task<List<GameDTO>> GetUserPassedGames(string userId)
        {
            var passedGames = new List<GameDTO>();
            var userRecords = await _dal.GetUserGameRecordsByUserId(userId);
            foreach (var record in userRecords)
            {
                var game = await _dal.GetGameById(record.GameId);
                if (record.Score == game.MaxScore)
                {
                    var gameDTO = _gameMapper.Map<Games, GameDTO>(game);
                    passedGames.Add(gameDTO);
                }
            }

            return passedGames;
        }

        public void RemoveGameRecordDTO(UserGameRecordDTO gameRecordDTO)
        {
            var record = _userGameRecordMapper.Map<UserGameRecordDTO, UserGameRecords>(gameRecordDTO);
            _dal.RemoveUserGameRecords(record);
        }

        public async Task UpdateUser(UserDTO user)
        {
            var userEntity = _userMapper.Map<UserDTO, AspNetUsers>(user);
            await _dal.UpdateUser(userEntity);
        }

        public async Task UpdateUserGameRecord(string userId, Guid gameId, int newScore)
        {
            var record = await _dal.GetUserGameRecordsByUserIdAndGameId(userId, gameId);
            if (record == null)
            {
                await _dal.AddUserGameRecords(new UserGameRecords
                {
                    Id = Guid.NewGuid(),
                    GameId = gameId,
                    UserId = userId,
                    Score = newScore,
                    TimesPlayed = 1
                });
            }
            else
            {
                if (newScore > record.Score)
                {
                    var game = await _dal.GetGameById(gameId);
                    if (newScore <= game.MaxScore)
                    {
                        int newTimesPlayed = record.TimesPlayed + 1;
                        await _dal.UpdateUserGameRecords(record.Id, newScore, newTimesPlayed);
                    }
                    else
                    {
                        throw new ArithmeticException();
                    }
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        public async Task UpdateUserProfilePicture(string userId, string picture)
        {
            await _dal.UpdateUserProfilePicture(userId, picture);
        }

        public async Task UpdateUserUserName(string userId, string userName)
        {
            if (_dal.GetUserByUserName(userName).Result == null)
            {
                await _dal.UpdateUserUserName(userId, userName);
            }
            else
            {
                throw new AggregateException();
            }
        }

        private void SetupMappers()
        {
            var userMapperConfig = new MapperConfiguration(
                cfg => cfg.CreateMap<UserDTO, AspNetUsers>().ReverseMap()
            );
            _userMapper = new Mapper(userMapperConfig);

            var gameMapperConfig = new MapperConfiguration(
                cfg => cfg.CreateMap<GameDTO, Games>().ReverseMap()
            );
            _gameMapper = new Mapper(gameMapperConfig);

            var userGameRecordConfig = new MapperConfiguration(
                cfg => cfg.CreateMap<UserGameRecordDTO, UserGameRecords>().ReverseMap()
            );
            _userGameRecordMapper = new Mapper(userGameRecordConfig);

            var themeConfig = new MapperConfiguration(
                cfg => cfg.CreateMap<ThemeDTO, Themes>().ReverseMap()
            );
            _themeMapper = new Mapper(themeConfig);

            var difficultyConfig = new MapperConfiguration(
                cfg => cfg.CreateMap<DifficultyDTO, Difficulties>().ReverseMap()
            );
            _difficultyMapper = new Mapper(difficultyConfig);
        }
    }
}
