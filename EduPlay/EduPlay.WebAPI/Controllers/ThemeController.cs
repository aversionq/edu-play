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
    public class ThemeController : ControllerBase
    {
        private readonly IEduPlayBLL _bll;

        public ThemeController(IEduPlayBLL bll)
        {
            _bll = bll;
        }

        [HttpGet]
        public async Task<List<ThemeDTO>> GetAllThemes()
        {
            return await _bll.GetAllThemes();
        }

        [HttpGet("{id}")]
        public async Task<ThemeDTO> GetThemeById(Guid id)
        {
            return await _bll.GetThemeById(id);
        }

        [HttpGet]
        [Route("getThemeByName")]
        public async Task<ThemeDTO> GetThemeByName(string name)
        {
            return await _bll.GetThemeByName(name);
        }
    }
}
