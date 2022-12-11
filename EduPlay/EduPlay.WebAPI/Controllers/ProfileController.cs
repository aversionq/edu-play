using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EduPlay.BLL.Interfaces;
using EduPlay.BLL.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using EduPlay.WebAPI.Auth;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

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
        private readonly IConfiguration _configuration;

        public ProfileController(IWebHostEnvironment webHostEnvironment, 
            UserManager<ApplicationUser> userManager, IEduPlayBLL bll, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _bll = bll;
            _userManager = userManager;
            _configuration = configuration;
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

        [HttpPut]
        [Route("updateUserProfilePicture")]
        public async Task<string> UpdateUserProfilePicture([FromForm] ProfilePictureUpload pfp)
        {
            try
            {
                if (pfp.files.Length > 0)
                {
                    var currentUserId = GetCurrentUser().Result.Id;
                    const string imgExpiration = "15552000";

                    using (var ms = new MemoryStream())
                    {
                        await pfp.files.CopyToAsync(ms);
                        var imageBytes = ms.ToArray();
                        string byteString = Convert.ToBase64String(imageBytes);
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri("https://api.imgbb.com");
                            var content = new[]
                            {
                                new KeyValuePair<string, string>("expiration", imgExpiration),
                                new KeyValuePair<string, string>("key", _configuration["ImageCDN:APIKey"]),
                                new KeyValuePair<string, string>("image", byteString)
                            };

                            var encodedItems = content.Select(i => WebUtility.UrlEncode(i.Key) + "=" + WebUtility.UrlEncode(i.Value));
                            var encodedContent = new StringContent(String.Join("&", encodedItems), null, "application/x-www-form-urlencoded");

                            var result = await client.PostAsync("/1/upload", encodedContent);
                            var response = await result.Content.ReadAsStreamAsync();

                            using var sr = new StreamReader(response);
                            using var jr = new JsonTextReader(sr);
                            JsonSerializer serializer = new JsonSerializer();

                            try
                            {
                                dynamic jsonResponse = serializer.Deserialize(jr);
                                var imageUrlDynamic = jsonResponse.data.url;
                                var imageUrlString = Convert.ToString(imageUrlDynamic);
                                await _bll.UpdateUserProfilePicture(currentUserId, imageUrlString);
                            }
                            catch (JsonReaderException)
                            {
                                return "Failed to upload profile picture";
                            }
                        }
                        ms.Flush();

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
            catch (AggregateException)
            {
                return BadRequest("This username is already taken");
            }
            catch (Exception)
            {
                return BadRequest("Some error happened while updating username");
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
                        return BadRequest("Password rules have been violated");
                    }
                }
                else
                {
                    return BadRequest("Wrong old password");
                }
            }
            else
            {
                return BadRequest("User not found");
            }
        }
    }
}
