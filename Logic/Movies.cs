using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Eagle1.Model;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;

namespace Eagle1.Logic
{
    public class Movies
    {
        public IEnumerable<MovieRecord> GetAll()
        {
            using TextFieldParser parser = new TextFieldParser(@"Data\metadata.csv")
            {
                TextFieldType = FieldType.Delimited
            };
            parser.SetDelimiters(",");
            var movies = new List<MovieRecord>();
            while (!parser.EndOfData) 
            {
                try
                {
                    string[] fields = parser.ReadFields();

                    var movie = new MovieRecord()
                    {
                        Id = int.Parse(fields[0]),
                        MovieId = int.Parse(fields[1]),
                        Title = fields[2],
                        Language = fields[3],
                        Duration = fields[4],
                        ReleaseYear = int.Parse(fields[5]),
                    };

                    if (!string.IsNullOrWhiteSpace(movie.Title) &&
                        !string.IsNullOrWhiteSpace(movie.Language) &&
                        !string.IsNullOrWhiteSpace(movie.Duration))
                    {
                        movies.Add(movie);
                    }
                }
                catch (FormatException) // if we cannot parse an integer
                {
                    // just ignore this line
                }
                catch (MalformedLineException) // if we cannot parse the line
                {
                    parser.ReadLine(); // just ignore this line
                }
            }

            return movies;
        }

        public IEnumerable<MovieRecord> GetById(int id)
        {
            var moviesByLang = new Dictionary<(int, string), MovieRecord>();
            foreach (var movie in GetAll())
            {
                var key = (movie.MovieId, movie.Language);
                if (!moviesByLang.ContainsKey(key) || moviesByLang[key].Id < movie.Id)
                    moviesByLang[key] = movie; // so we end up with highest Id for each language
            }
            var result = moviesByLang.Values.Where(m => m.MovieId == id);
            return result;
        }

        public int GetNextId()
        {
            return GetAll().Select(m => m.Id).Max() + 1;
        }
    }
}