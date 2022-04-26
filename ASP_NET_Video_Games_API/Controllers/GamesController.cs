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
        
        [HttpGet("gamesByConsole")]
        //since 2013
        public IActionResult GetSalesByConsole()
        {
            //global video games past 2013 by console.
            var videoGames = _context.VideoGames.Where(vg => vg.Year > 2013);
            var consoles = _context.VideoGames.Select(c => c.Platform).Distinct();
            //int i = 0;
            Dictionary<string, double> returnValue = new Dictionary<string, double>();
            foreach(string Platform in consoles)
            {
                var salesPerConsole = videoGames.Where(i => i.Platform == Platform).Select(i => i.GlobalSales).Sum();
                returnValue.Add(Platform, salesPerConsole);
            }
            return Ok(returnValue);
        }
    }
}
