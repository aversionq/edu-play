using System;
using System.Collections.Generic;
using System.Text;
using EduPlay.DAL.Interfaces;
using EduPlay.DAL.Entities;
using EduPlay.BLL.Models;
using EduPlay.BLL.Interfaces;
using AutoMapper;
using System.Threading.Tasks;

namespace EduPlay.BLL
{
    public class EduPlayBLL : IEduPlayBLL
    {
        private IEduPlayDAL _dal;
        private Mapper _userMapper;
        private Mapper _gameMapper;
        private Mapper _userGameRecordMapper;

        public EduPlayBLL(IEduPlayDAL dal)
        {
            _dal = dal;
            SetupMappers();
        }

        public async Task AddUserGameRecord(UserGameRecordDTO gameRecordDTO)
        {
            var gameRecord = _userGameRecordMapper.Map<UserGameRecordDTO, UserGameRecords>(gameRecordDTO);
            await _dal.AddUserGameRecords(gameRecord);
        }

        public async Task<List<GameDTO>> GetAllGames()
        {
            var games = await _dal.GetAllGames();
            List<GameDTO> gamesDto = _gameMapper.Map<List<Games>, List<GameDTO>>(games);
            return gamesDto;
        }

        public List<UserDTO> GetAllUsers()
        {
            var users = _dal.GetAllUsers();
            var usersDto = _userMapper.Map<List<AspNetUsers>, List<UserDTO>>(users);
            return usersDto;
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

        //public Task<UserDTO> GetUserByUserName(string userName)
        //{
        //    _dal.
        //}

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

        public async Task UpdateUserGameRecord(UserGameRecordDTO newGameRecordDTO)
        {
            var gameRecord = _userGameRecordMapper.Map<UserGameRecordDTO, UserGameRecords>(newGameRecordDTO);
            await _dal.UpdateUserGameRecords(gameRecord);
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
                throw new Exception("This username is already taken.");
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
        }
    }
}
