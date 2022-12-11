using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EduPlay.BLL.Interfaces;
using EduPlay.BLL.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace EduPlay.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly IEduPlayBLL _bll;

        public LeaderboardController(IEduPlayBLL bll)
        {
            _bll = bll;
        }

        [HttpGet]
        [Route("getUserGameRecordsByGameId")]
        public async Task<List<UserGameRecordDTO>> GetUserGameRecordsByGameId(Guid id)
        {
            return await _bll.GetUserGameRecordsByGameId(id);
        }
    }
}
