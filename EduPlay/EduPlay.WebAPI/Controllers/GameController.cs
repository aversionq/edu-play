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
        public async Task<ActionResult> UpdateUserGameRecord(UserGameRecordDTO record)
        {
            await _bll.UpdateUserGameRecord(record);
            return Ok(record);
        }
    }
}
