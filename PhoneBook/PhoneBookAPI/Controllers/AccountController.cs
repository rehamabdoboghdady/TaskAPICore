using BL.AppServices;
using BL.ViewModels;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PhoneBookAPI.HelpClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhoneBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IConfiguration _config;
        private AccountAppService _accountAppservice;
        IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController
            (
              IConfiguration config,
              AccountAppService accountAppservice,
              IHttpContextAccessor httpContextAccessor,
               UserManager<User> userManager,
               RoleManager<IdentityRole> roleManager
            )
        {
            _config = config;
            _accountAppservice = accountAppservice;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            IActionResult response = Unauthorized();
            var user = await _accountAppservice.Find(login.Email, login.Password);
            if (user != null)
            {
                var tokenString = _accountAppservice.CreateToken(user);
                response = Ok(new { token = tokenString });
            }
            return response;
        }
        [HttpPost("/Register")]
        public async Task<IActionResult> Register(RegisterViewModel userAccount)
        {
            IdentityRole role = new IdentityRole("User");
            var roles = await _roleManager.CreateAsync(role);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            userAccount.Role = "User";
            var user = await _accountAppservice.Register(userAccount);

            if (user.Succeeded)
            {
                var currentUser = await _accountAppservice.FindByName(userAccount.UserName);
                var userId = currentUser.Id;
                await _accountAppservice.AssignToRole(userId, "User");
                return Ok();
            }
            else
                return BadRequest(user.Errors.ToList()[0]);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AdminRegister")]
        public async Task<IActionResult> RegisterAdmin(RegisterViewModel userAccount)
        {
            IdentityRole role = new IdentityRole("Admin");
            var roles = await _roleManager.CreateAsync(role);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            userAccount.Role = "Admin";
            var user = await _accountAppservice.Register(userAccount);
            if (user.Succeeded)
            {
                var currentUser = await _accountAppservice.FindByName(userAccount.UserName);
                var userId = currentUser.Id;
                await _accountAppservice.AssignToRole(userId, "Admin");
                return Ok();
            }
            else
                return BadRequest(user.Errors.ToList()[0]);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            var res = _accountAppservice.GetAllAccounts();
            return Ok(res);
        }
        [HttpGet("{id}")]
        public IActionResult GetUserById(string id)
        {
            var res = _accountAppservice.GetAccountById(id);
            return Ok(res);
        }
        [HttpGet("GetUserName/{id}")]
        public IActionResult GetUserName(string id)
        {
            var res = _accountAppservice.GetAccountById(id);
            return Ok(res.UserName);
        }

        [Authorize]
        [HttpGet("current")]
        public IActionResult GetCurrentUser()
        {
            var userID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var res = _accountAppservice.GetAccountById(userID);
            return Ok(res);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, RegisterViewModel registerViewodel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _accountAppservice.Update(registerViewodel);

            return Ok(new Response { Status = "Success", Message = "User updated successfully!" });

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                _accountAppservice.DeleteAccount(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ChangePasswordAsync(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ChangePasswordAsync(user, model.currentPassword, model.newPassword);

            if (!result.Succeeded)
            {
                return Ok("Password can not change");
            }
            return Ok(result);
        }

    }
}
