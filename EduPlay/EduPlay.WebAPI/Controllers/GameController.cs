using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EduPlay.BLL.Interfaces;
using EduPlay.BLL.Models;
using EduPlay.Dependencies;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace EduPlay.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IEduPlayBLL _bll;

        public GameController()
        {
            _bll = DependencyResolver.Instance.EduPlayBLL;
        }

        [HttpGet]
        [Route("getAllGames")]
        public async Task<List<GameDTO>> GetAllGames()
        {
            return await _bll.GetAllGames();
        }

        [HttpGet]
        [Route("getGamesByDifficultyId")]
        public async Task<List<GameDTO>> GetGamesByDifficultyId(Guid id)
        {
            return await _bll.GetGamesByDifficultyId(id);
        }

        [HttpGet]
        [Route("getGamesByThemeId")]
        public async Task<List<GameDTO>> GetGamesByThemeId(Guid id)
        {
            return await _bll.GetGamesByThemeId(id);
        }

        [HttpGet]
        [Route("getGameById")]
        public async Task<GameDTO> GetGameById(Guid id)
        {
            return await _bll.GetGameById(id);
        }

        [HttpPut]
        [Route("updateUserGameRecord")]
        public async Task<ActionResult> UpdateUserGameRecord(string userId, Guid gameId, int newScore)
        {
            //try
            //{
                await _bll.UpdateUserGameRecord(userId, gameId, newScore);
                return Ok();
            //}
            //catch (Exception)
            //{
            //    return BadRequest("Some error happened while updating user's score");
            //}
        }

        [HttpGet]
        [Route("getAllThemes")]
        public async Task<List<ThemeDTO>> GetAllThemes()
        {
            return await _bll.GetAllThemes();
        }

        [HttpGet]
        [Route("getAllDifficulties")]
        public async Task<List<DifficultyDTO>> GetAllDifficulties()
        {
            return await _bll.GetAllDifficulties();
        }

        [HttpGet]
        [Route("getThemeById")]
        public async Task<ThemeDTO> GetThemeById(Guid id)
        {
            return await _bll.GetThemeById(id);
        }

        [HttpGet]
        [Route("getDifficultyById")]
        public async Task<DifficultyDTO> GetDifficultyById(Guid id)
        {
            return await _bll.GetDifficultyById(id);
        }

        [HttpGet]
        [Route("getThemeByName")]
        public async Task<ThemeDTO> GetThemeByName(string name)
        {
            return await _bll.GetThemeByName(name);
        }

        [HttpGet]
        [Route("getDifficultyByValue")]
        public async Task<DifficultyDTO> GetDifficultyByValue(int value)
        {
            return await _bll.GetDifficultyByValue(value);
        }

        [HttpPut]
        [Route("UpdateTimesPlayed")]
        public async Task<ActionResult> UpdateTimesPlayed(string userId, Guid gameId)
        {
            try
            {
                await _bll.UpdateTimesPlayed(userId, gameId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Some error happened while updating times played parameter");
            }
        }

        [HttpPost]
        [Route("AddUserGameRecord")]
        public async Task<ActionResult> AddUserGameRecord(UserGameRecordDTO record)
        {
            try
            {
                await _bll.AddUserGameRecord(record);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Some error happened while adding new game record for user");
            }
        }

        [HttpGet]
        [Route("GenerateGuid")]
        public ActionResult<Guid> GenerateGuid()
        {
            return Ok(Guid.NewGuid());
        }
    }
}
