using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EduPlay.BLL.Interfaces;
using EduPlay.BLL.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EduPlay.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IEduPlayBLL _bll;

        public GameController(IEduPlayBLL bll)
        {
            _bll = bll;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List<GameDTO>> GetAllGames()
        {
            return await _bll.GetAllGames();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getGamesByDifficultyId")]
        public async Task<List<GameDTO>> GetGamesByDifficultyId(Guid id)
        {
            return await _bll.GetGamesByDifficultyId(id);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getGamesByThemeId")]
        public async Task<List<GameDTO>> GetGamesByThemeId(Guid id)
        {
            return await _bll.GetGamesByThemeId(id);
        }

        [HttpGet("{id}")]
        public async Task<GameDTO> GetGameById(Guid id)
        {
            return await _bll.GetGameById(id);
        }

        private string GetCurrentUserId()
        {
            ClaimsPrincipal currentUser = this.User;
            return currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [HttpPut]
        [Route("updateGameRecord")]
        public async Task<ActionResult> GameRecord(Guid gameId, [FromBody] int newScore)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _bll.UpdateUserGameRecord(userId, gameId, newScore);
                return Ok();
            }
            catch (ArithmeticException)
            {
                return BadRequest("The new score value is larger than game max score");
            }
            catch (ArgumentException)
            {
                return BadRequest("New score is lower than the older score");
            }
            catch (Exception)
            {
                return BadRequest("Some error happened while updating user's score");
            }
        }

        [HttpGet]
        [Route("CheckUserAge")]
        public async Task<ActionResult> CheckUserAge(Guid gameId)
        {
            var game = await _bll.GetGameById(gameId);
            var currentUser = await _bll.GetUserById(GetCurrentUserId());
            int userAge = (int)((DateTime.Now - currentUser.DateOfBirth).TotalDays / 365.25);
            if (userAge >= game.AgeLimit)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Age limit not passed");
            }
        }
    }
}
