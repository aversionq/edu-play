using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EduPlay.BLL.Interfaces;
using EduPlay.BLL.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace EduPlay.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DifficultyController : ControllerBase
    {
        private readonly IEduPlayBLL _bll;

        public DifficultyController(IEduPlayBLL bll)
        {
            _bll = bll;
        }

        [HttpGet]
        public async Task<List<DifficultyDTO>> GetAllDifficulties()
        {
            return await _bll.GetAllDifficulties();
        }

        [HttpGet("{id}")]
        public async Task<DifficultyDTO> GetDifficultyById(Guid id)
        {
            return await _bll.GetDifficultyById(id);
        }

        [HttpGet]
        [Route("getDifficultyByValue")]
        public async Task<DifficultyDTO> GetDifficultyByValue(int value)
        {
            return await _bll.GetDifficultyByValue(value);
        }
    }
}
