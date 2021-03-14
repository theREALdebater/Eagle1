using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle1.Logic;
using Eagle1.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Eagle1.Controllers
{
    [ApiController]
    [Route("movies")]
    public class MoviesController : ControllerBase
    {
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(ILogger<MoviesController> logger)
        {
            _logger = logger;
        }

        [HttpGet("stats")]
        public IEnumerable<MovieStats> GetStats()
        {
            var stats = new StatsCalc()
                .CalcStats()
                .OrderByDescending(r => r.AverageWatchDurationS)
                .ThenByDescending(r => r.ReleaseYear);
            
            return stats;
        }
        
    }
}