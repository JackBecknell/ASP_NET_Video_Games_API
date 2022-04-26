using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASP_NET_Video_Games_API.Data;
using ASP_NET_Video_Games_API.Models;
using System.Collections.Generic;

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
            var consoles = _context.VideoGames.Select(c => c.Platform).Distinct();
            //int i = 0;
            Dictionary<string, double> returnValue = new Dictionary<string, double>();
            foreach(string Platform in consoles.ToList())
            {
                var salesPerConsole = _context.VideoGames.Where(i => i.Platform == Platform).Where(vg => vg.Year > 2013).Select(i => i.GlobalSales).Sum();
                returnValue.Add(Platform, salesPerConsole);
            }
            return Ok(returnValue);
        }

        [HttpGet("gamebyname/{name}")]
        public IActionResult GetGamesByName(string name)
        {
            var videoGame = _context.VideoGames.Where(vg => vg.Name == name);
            
            return Ok(videoGame);
        }

        [HttpGet("bestGamesYearly")]
        public IActionResult GetBestGames()
        {
            var years = _context.VideoGames.Where(c => c.Year > 2013).Select(c => c.Year).Distinct();
            List<IQueryable> returnValue = new List<IQueryable>();
            foreach (int year in years.ToList())
            {
                var highestSalesPerYr = _context.VideoGames.Where(i => i.Year == year).Max(vg => vg.GlobalSales);
                var gameWthHighestSales = _context.VideoGames.Where(i => i.GlobalSales == highestSalesPerYr && i.Year == year);
                returnValue.Add(gameWthHighestSales);
            }
            return Ok(returnValue);
        }


        //[HttpGet("salesByPublisher")]
        ////since 2013
        //public IActionResult GetSalesByConsole()
        //{
        //    //global video games past 2013 by console.
        //    var publishers = _context.VideoGames.Select(c => c.Publisher).Distinct();
        //    //int i = 0;
        //    Dictionary<string, double> returnValue = new Dictionary<string, double>();
        //    foreach (string Publisher in publishers.ToList())
        //    {
        //        var salesPerPublisher = _context.VideoGames.Where(i => i.Publisher == Publisher).Where(vg => vg.Year > 2013).Select(i => i.GlobalSales).Sum();
        //        returnValue.Add(Platform, salesPerConsole);
        //    }
        //    return Ok(returnValue);
        //}
    }
}
