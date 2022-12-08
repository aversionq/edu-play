using EduPlay.WebAPI.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.WebUtilities;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace EduPlay.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private const string _defaultAvatar = "https://i.ibb.co/NCBX5N4/265ee71be335.jpg";

        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExist = await _userManager.FindByEmailAsync(model.Email) ??
                await _userManager.FindByNameAsync(model.UserName);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "User already exists");
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                Name = model.Name,
                DateOfBirth = model.DateOfBirth.Date,
                Surname = model.Surname,
                SecurityStamp = Guid.NewGuid().ToString(),
                IsBanned = false,
                UserName = model.UserName,
                ProfilePicture = _defaultAvatar
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating a user");
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.DefaultUser))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.DefaultUser));
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.PremiumUser))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.PremiumUser));
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.DefaultUser);
            }

            return Ok("User created");
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExist = await _userManager.FindByEmailAsync(model.Email) ??
                await _userManager.FindByNameAsync(model.UserName);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "User already exists");
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                Name = model.Name,
                DateOfBirth = model.DateOfBirth.Date,
                Surname = model.Surname,
                SecurityStamp = Guid.NewGuid().ToString(),
                IsBanned = false,
                UserName = model.UserName,
                ProfilePicture = _defaultAvatar
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating a user");
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.DefaultUser))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.DefaultUser));
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.PremiumUser))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.PremiumUser));
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok("Admin created");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.UserName) ??
                await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256),
                    expires: DateTime.Now.AddDays(20)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("forgetPassword")]
        public async Task<ActionResult> ForgetPassword([FromBody] ForgetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = Encoding.UTF8.GetBytes(token);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);

                var url = $"{_configuration["AppUrl"]}/ResetPassword?email={model.Email}&token={validToken}";

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration["MailSettings:Address"]));
                email.To.Add(MailboxAddress.Parse(model.Email));
                email.Subject = $"EduPlay password reset";
                email.Body = new TextPart(TextFormat.Html) 
                { 
                    Text = $"<h1>You requested a password recovery!</h1>" +
                    $"<p>Link to recover your password: {url}</p>"
                };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.mailosaur.net", 2525, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_configuration["MailSettings:Address"],
                    _configuration["MailSettings:Password"]);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return Ok($"Email with recovery link sent to {model.Email}");
            }
            else
            {
                return BadRequest("User not found");
            }
        }

        [HttpPut]
        [Route("resetPassword")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var decodedToken = WebEncoders.Base64UrlDecode(model.ResetToken);
                var decoder = Encoding.UTF8.GetDecoder();

                char[] chars;
                int charCount = decoder.GetCharCount(decodedToken, 0, decodedToken.Length);
                chars = new Char[charCount];
                int charsDecodedCount = decoder.GetChars(decodedToken, 0, decodedToken.Length, chars, 0);
                var validToken = new string(chars);

                var result = await _userManager.ResetPasswordAsync(user, validToken, model.Password);
                if (result.Succeeded)
                {
                    return Ok("Password has been reset successfully");
                }
                else
                {
                    return BadRequest("An unexpected error happened while reseting password");
                }
            }
            else
            {
                return BadRequest("User not found");
            }
        }
    }
}
