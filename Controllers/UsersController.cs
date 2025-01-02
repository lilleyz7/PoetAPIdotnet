using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoetAPI.Repos;
using PoetAPI.Models;
using PoetAPI.DTOs;

namespace PoetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserRepo _userRepo;

        public UsersController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        //get user

        [HttpGet("GetUser")]
        public IActionResult GetUser(string id)
        {
            try
            {
                return Ok(_userRepo.GetSingleUser(id));
            }
            catch(Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        //[HttpGet("AllUsers")]
        //public IActionResult GetUsers()
        //{
        //    try
        //    {
        //        return Ok(_userRepo.GetUsers());
        //    }
        //    catch(Exception e)
        //    {
        //        return BadRequest(e.ToString());
        //    }
        //}
        //update user
        //delete user
    }
}
