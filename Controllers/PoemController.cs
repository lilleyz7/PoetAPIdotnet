using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoetAPI.Repos;
using PoetAPI.Models;
using PoetAPI.DTOs;
using System.Security.Claims;

namespace PoetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoemController : ControllerBase
    {
        public PoemRepo _poemRepo;

        public PoemController()
        {
            _poemRepo = new PoemRepo();
        }

        [Authorize]
        [HttpPost("save")]
        public IActionResult SavePoem(PoemDTO poemToAdd)
        {
            string? user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user == null)
            {
                return Unauthorized();
            }
            Poem poem = new Poem { Title = poemToAdd.Title, Lines = poemToAdd.Lines };
            try
            {
                _poemRepo.SavePoem(poem, user);
                return Ok("Poem saved");
            }
            catch
            {
                return BadRequest("Poem not saved");

            }
        }
        [Authorize]
        [HttpPost("unsave")]
        public IActionResult UnsavePoem(int poemID)
        {
            string? user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user == null)
            {
                return Unauthorized();
            }
            try
            {
                _poemRepo.UnsavePoem(poemID, user);
                return Ok("Poem unsaved");
            }
            catch (Exception e)
            {
                return BadRequest(e);

            }
        }
        [HttpGet("searchByTitle/{title}")]
        public IActionResult SearchByName(string title)
        {
            try
            {
                PoemDTO poem = _poemRepo.SearchByPoemTitle(title).Result;
                return Ok(poem);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("random/{count}")]
        public IActionResult GetRandomPoems(int count)
        {
            try
            {
                if (count < 1 || count > 5)
                {
                    throw new Exception("Invalid return count. Must be between 1 and 5");
                }

                List<PoemDTO> poems = _poemRepo.GetRandomPoems(count).Result;
                return Ok(poems);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("authors/{authorName}")]
        public IActionResult SearchByAuthor(string authorName)
        {
            try
            {
                List<PoemDTO> poems = _poemRepo.SearchByAuthor(authorName).Result;
                return Ok(poems);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
