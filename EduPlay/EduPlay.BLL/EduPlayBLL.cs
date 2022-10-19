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
    internal class EduPlayBLL : IEduPlayBLL
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
            throw new NotImplementedException();
        }

        public List<GameDTO> GetAllGames()
        {
            throw new NotImplementedException();
        }

        public List<UserDTO> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public GameDTO GetGameById(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<GameDTO> GetGamesByDifficultyId(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<GameDTO> GetGamesByThemeId(Guid id)
        {
            throw new NotImplementedException();
        }

        public UserDTO GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public UserDTO GetUserById(string id)
        {
            throw new NotImplementedException();
        }

        public List<UserGameRecordDTO> GetUserGameRecordsByGameId(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<UserGameRecordDTO> GetUserGameRecordsByUserId(string id)
        {
            throw new NotImplementedException();
        }

        public void RemoveGameRecordDTO(UserGameRecordDTO gameRecordDTO)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserGameRecord(UserGameRecordDTO newGameRecordDTO)
        {
            throw new NotImplementedException();
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
