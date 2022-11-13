using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EduPlay.BLL.Interfaces;
using EduPlay.BLL.Models;
using EduPlay.Dependencies;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using EduPlay.WebAPI.Auth;

namespace EduPlay.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IEduPlayBLL _bll;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(IWebHostEnvironment webHostEnvironment, 
            UserManager<ApplicationUser> userManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _bll = DependencyResolver.Instance.EduPlayBLL;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("getUserGameRecords")]
        public async Task<ActionResult<List<UserGameRecordDTO>>> GetUserGameRecords()
        {
            try
            {
                return await _bll.GetUserGameRecordsByUserId(GetCurrentUserId());
            }
            catch (Exception)
            {
                return BadRequest("Some error happened while getting user game records");
            }
        }

        [HttpGet]
        [Route("getUserPassedGames")]
        public async Task<ActionResult<List<GameDTO>>> GetUserPassedGames()
        {
            try
            {
                return await _bll.GetUserPassedGames(GetCurrentUserId());
            }
            catch (Exception)
            {
                return BadRequest("Some error happened while getting user passed games");
            }
        }

        [HttpGet]
        [Route("getUserBestResult")]
        public async Task<ActionResult<GameDTO>> GetUserBestResult()
        {
            try
            {
                return await _bll.GetUserBestResult(GetCurrentUserId());
            }
            catch (Exception)
            {
                return BadRequest("Some error happened while trying to get user's best result");
            }
        }

        [HttpGet]
        [Route("getCurrentUser")]
        public async Task<UserDTO> GetCurrentUser()
        {
            var currentUserId = GetCurrentUserId();
            var currentUserDTO = await _bll.GetUserById(currentUserId);
            return currentUserDTO;
        }

        [HttpGet]
        [Route("getCurrentUserId")]
        public string GetCurrentUserId()
        {
            ClaimsPrincipal currentUser = this.User;
            return currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        //[HttpPut]
        //[Route("updateUser")]
        //public async Task<ActionResult> UpdateUser(UserDTO updatedUser)
        //{
        //    await _bll.UpdateUser(updatedUser);
        //    return Ok(updatedUser);
        //}

        [HttpPut]
        [Route("updateUserProfilePicture")]
        public async Task<string> UpdateUserProfilePicture([FromForm] ProfilePictureUpload pfp)
        {
            try
            {
                if (pfp.files.Length > 0)
                {
                    var path = _webHostEnvironment.WebRootPath + @"\Uploads\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var currentUserId = GetCurrentUser().Result.Id;
                    var picturePath = path + pfp.files.FileName;

                    using (FileStream fileStream = System.IO.File.Create(picturePath))
                    {
                        await pfp.files.CopyToAsync(fileStream);
                        await _bll.UpdateUserProfilePicture(currentUserId, picturePath);
                        fileStream.Flush();
                        return $"User profile picture uploaded";
                    }
                }
                else
                {
                    return "Failed to upload profile picture";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPut]
        [Route("updateUserUserName")]
        public async Task<ActionResult> UpdateUserUserName(string userName)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _bll.UpdateUserUserName(userId, userName);
                return Ok($"Username updated");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Some error happened while updating username");
            }
        }

        [HttpPut]
        [Route("updateUserPassword")]
        public async Task<ActionResult> UpdateUserPassword(string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(GetCurrentUserId());
            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, oldPassword))
                {
                    var _passwordValidator = HttpContext.RequestServices.GetService(typeof
                        (IPasswordValidator<ApplicationUser>)) as IPasswordValidator<ApplicationUser>;
                    var isPasswordValid = await _passwordValidator.ValidateAsync(_userManager, user, newPassword);
                    if (isPasswordValid.Succeeded)
                    {
                        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                        if (result.Succeeded)
                        {
                            return Ok("User password updated successfully");
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError,
                                "Unexpected error happened while updating password");
                        }
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError,
                            "Password rules have been violated");
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                            "Wrong old password");
                }
            }
            else
            {
                return BadRequest("User not found");
            }
        }
    }
}
