using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EduPlay.WebAPI.Auth;
using EduPlay.BLL.Interfaces;
using EduPlay.BLL.Models;
using EduPlay.Dependencies;
using System.Collections.Generic;
using System.Security.Claims;

namespace EduPlay.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEduPlayBLL _bll;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _bll = DependencyResolver.Instance.EduPlayBLL;
        }

        [HttpGet]
        [Route("getUserGameRecords")]
        public ActionResult<List<UserGameRecordDTO>> GetUserGameRecords(string id)
        {
            return Ok(_bll.GetUserGameRecordsByUserId(id));
        }

        [HttpGet]
        [Route("getCurrentUser")]
        public ActionResult<UserDTO> GetCurrentUser()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var currentUserDTO = _bll.GetUserById(currentUserId);
            return Ok(currentUserDTO);
        }

        [HttpPut]
        [Route("updateUser")]
        public ActionResult UpdateUser(UserDTO updatedUser)
        {
            _bll.UpdateUser(updatedUser);
            return Ok();
        }
    }
}
