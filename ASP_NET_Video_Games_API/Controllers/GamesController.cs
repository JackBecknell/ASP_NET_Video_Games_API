﻿using Microsoft.AspNetCore.Http;
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
                var salesPerConsole = _context.VideoGames.Where(i => i.Platform == Platform).Where(vg => vg.Year > 2013).Where(vg=>vg.GlobalSales > 0).Select(i => i.GlobalSales).Sum();
                if (salesPerConsole > 0)
                    {
                        returnValue.Add(Platform, salesPerConsole);
                    }
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
            var years = _context.VideoGames.Where(c => c.Year != null).Select(c => c.Year).Distinct();
            Dictionary<double, string> returnValue = new Dictionary<double, string>();
            foreach (int year in years.ToList())
            {
                var highestSalesPerYr = _context.VideoGames.Where(i => i.Year == year).Max(vg => vg.GlobalSales);
                var gameWthHighestSales = _context.VideoGames.Where(i => i.GlobalSales == highestSalesPerYr && i.Year == year).Select(i => i.Name).First().ToString();
                returnValue.Add(year, gameWthHighestSales);
            }

            return Ok(returnValue);
        }

        [HttpGet("publisherSalesByConsole")]
        public IActionResult GetPublisherSalesByConsole()
        {
            var consoles = _context.VideoGames.Select(c => c.Platform).Distinct();
            Dictionary<string, Dictionary<string, double>> returnValue = new Dictionary<string, Dictionary<string, double>>();  
            foreach (string console in consoles.ToList())
            {
                Dictionary<string, double> sales = new Dictionary<string, double>();
                var publishers = _context.VideoGames.Where(c => c.Platform == console).OrderByDescending(c => c.GlobalSales).Select(c => c.Publisher).Distinct().Take(3);
                foreach (string publisher in publishers.ToList())
                {
                    var globalSales = _context.VideoGames.Where(vg => vg.Publisher == publisher).Select(i => i.GlobalSales).Sum();
                    sales.Add(publisher, globalSales);
                }
                returnValue.Add(console, sales);
            }
            return Ok(returnValue);
        }



    }
}
