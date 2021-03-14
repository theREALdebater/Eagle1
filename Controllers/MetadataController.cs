using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Eagle1.Logic;
using Eagle1.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Eagle1.Controllers
{
    [ApiController]
    [Route("metadata")]
    public class MetadataController : ControllerBase
    {
        private readonly ILogger<MetadataController> _logger;

        public List<MovieRecord> Database = new List<MovieRecord>();

        public MetadataController(ILogger<MetadataController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public MovieRecord AddRecord([FromBody] MovieRecord movieRec)
        {
            movieRec.Id = new Movies().GetNextId();
            Database.Add(movieRec);
            return movieRec;
        }

        [HttpGet("{movieId}")]
        public IActionResult GetRecords(int movieId)
        {
            var movies = new Movies().GetById(movieId).OrderBy(m => m.Language).ToList();
            if (movies.Count == 0) return new NotFoundResult();
            return new JsonResult(movies);
        }

    }
}