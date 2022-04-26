using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASP_NET_Video_Games_API.Data;

namespace ASP_NET_Video_Games_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        //insantiating database info into variable
        private readonly ApplicationDbContext _context;

        //creating contructor
        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetGames()
        {
            var videoGames = _context.VideoGames;
            return Ok(videoGames);
        }

        [HttpGet("{id}")]
        public IActionResult GetGamesById(int id)
        {
            var videoGames = _context.VideoGames.Where(vg => vg.Id == id);
            return Ok(videoGames);
        }

        [HttpGet("search/{searchTerm}")]
        public IActionResult GetGames(string searchTerm)
        {
            var videoGames = _context.VideoGames.Where(vg => vg.Name.Contains(searchTerm));
            return Ok(videoGames);
        }
    }
}
