using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PoetAPI.DTOs;
using PoetAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using PoetAPI.Data;

namespace PoetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signinManager;

        public AuthController(UserManager<CustomUser> userManager, SignInManager<CustomUser> signinManger)
        {
            _userManager = userManager;
            _signinManager = signinManger;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegistrationDTO model)
        {
            var user = new CustomUser { UserName = model.Username, Email = model.Email };
            Console.WriteLine(user.UserName);
            Console.WriteLine(user.Email);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok("Registration successful");
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO model)
        {
            var result = await _signinManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if (result.Succeeded)
            {
                var token = JWTUtil.GenerateJwtToken(model.Username);
                return Ok(new {token});
            }
            return Unauthorized("Invalid email or password");
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return Ok("Logged out");
        }

        
    }
}
