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

namespace EduPlay.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IEduPlayBLL _bll;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _bll = DependencyResolver.Instance.EduPlayBLL;
        }

        [HttpGet]
        [Route("getUserGameRecords")]
        public async Task<List<UserGameRecordDTO>> GetUserGameRecords(string id)
        {
            return await _bll.GetUserGameRecordsByUserId(id);
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

        [HttpPut]
        [Route("updateUser")]
        public async Task<ActionResult> UpdateUser(UserDTO updatedUser)
        {
            await _bll.UpdateUser(updatedUser);
            return Ok(updatedUser);
        }

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
                        return $"{currentUserId} Profile picture uploaded.";
                    }
                }
                else
                {
                    return "Failed to upload profile picture.";
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
                return Ok($"Username for user {userId} updated");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Some error happened while updating username.");
            }
        }
    }
}
