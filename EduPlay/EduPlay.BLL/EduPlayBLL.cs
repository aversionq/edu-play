using System;
using System.Collections.Generic;
using System.Text;
using EduPlay.DAL.Interfaces;
using EduPlay.DAL.Entities;
using EduPlay.BLL.Models;
using EduPlay.BLL.Interfaces;
using AutoMapper;

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

        public void AddUserGameRecord(UserGameRecordDTO gameRecordDTO)
        {
            var gameRecord = _userGameRecordMapper.Map<UserGameRecordDTO, UserGameRecords>(gameRecordDTO);
            _dal.AddUserGameRecords(gameRecord);
        }

        public List<GameDTO> GetAllGames()
        {
            var games = _dal.GetAllGames();
            List<GameDTO> gamesDto = _gameMapper.Map<List<Games>, List<GameDTO>>(games);
            return gamesDto;
        }

        public List<UserDTO> GetAllUsers()
        {
            var users = _dal.GetAllUsers();
            var usersDto = _userMapper.Map<List<AspNetUsers>, List<UserDTO>>(users);
            return usersDto;
        }

        public GameDTO GetGameById(Guid id)
        {
            var game = _dal.GetGameById(id);
            var gameDto = _gameMapper.Map<Games, GameDTO>(game);
            return gameDto;
        }

        public List<GameDTO> GetGamesByDifficultyId(Guid id)
        {
            var games = _dal.GetGamesByDifficultyId(id);
            var gamesDto = _gameMapper.Map<List<Games>, List<GameDTO>>(games);
            return gamesDto;
        }

        public List<GameDTO> GetGamesByThemeId(Guid id)
        {
            var games = _dal.GetGamesByThemeId(id);
            var gamesDto = _gameMapper.Map<List<Games>, List<GameDTO>>(games);
            return gamesDto;
        }

        public UserDTO GetUserByEmail(string email)
        {
            var user = _dal.GetUserByEmail(email);
            var userDto = _userMapper.Map<AspNetUsers, UserDTO>(user);
            return userDto;
        }

        public UserDTO GetUserById(string id)
        {
            var user = _dal.GetUserById(id);
            var userDto = _userMapper.Map<AspNetUsers, UserDTO>(user);
            return userDto;
        }

        public List<UserGameRecordDTO> GetUserGameRecordsByGameId(Guid id)
        {
            var records = _dal.GetUserGameRecordsByGameId(id);
            var recordsDto = _userGameRecordMapper.Map<List<UserGameRecords>, List<UserGameRecordDTO>>(records);
            return recordsDto;
        }

        public List<UserGameRecordDTO> GetUserGameRecordsByUserId(string id)
        {
            var users = _dal.GetUserGameRecordsByUserId(id);
            var usersDto = _userGameRecordMapper.Map<List<UserGameRecords>, List<UserGameRecordDTO>>(users);
            return usersDto;
        }

        public void RemoveGameRecordDTO(UserGameRecordDTO gameRecordDTO)
        {
            var record = _userGameRecordMapper.Map<UserGameRecordDTO, UserGameRecords>(gameRecordDTO);
            _dal.RemoveUserGameRecords(record);
        }

        public void UpdateUser(UserDTO user)
        {
            var userEntity = _userMapper.Map<UserDTO, AspNetUsers>(user);
            _dal.UpdateUser(userEntity);
        }

        public void UpdateUserGameRecord(UserGameRecordDTO newGameRecordDTO)
        {
            var gameRecord = _userGameRecordMapper.Map<UserGameRecordDTO, UserGameRecords>(newGameRecordDTO);
            _dal.UpdateUserGameRecords(gameRecord);
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
