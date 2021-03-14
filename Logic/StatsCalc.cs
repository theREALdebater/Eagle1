using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Eagle1.Model;
using Microsoft.VisualBasic.FileIO;

namespace Eagle1.Logic
{
    public struct ViewingInstance
    {
        public int MovieId;
        public long WatchDurationMs;
    }
    
    public class StatsCalc
    {
        public IEnumerable<ViewingInstance> GetInstances()
        {
            using TextFieldParser parser = new TextFieldParser(@"Data\stats.csv")
            {
                TextFieldType = FieldType.Delimited
            };
            parser.SetDelimiters(",");
            var instances = new List<ViewingInstance>();
            while (!parser.EndOfData) 
            {
                try
                {
                    string[] fields = parser.ReadFields();

                    var instance = new ViewingInstance()
                    {
                        MovieId = int.Parse(fields[0]),
                        WatchDurationMs = long.Parse(fields[1]),
                    };

                    instances.Add(instance);
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

            return instances;
        }

        public IEnumerable<MovieStats> CalcStats()
        {
            var movies = new Movies().GetAll().ToList();
            var instances = GetInstances();
            var result = new Dictionary<int, MovieStats>();
            MovieStats rec;
            string title = "(unknown)";
            int releaseYear = 0;
            bool movieFound;

            foreach (var i in instances)
            {
                if (result.ContainsKey(i.MovieId))
                {
                    // stats record already exists, so just update it 
                    rec = result[i.MovieId];
                    rec.AverageWatchDurationS += i.WatchDurationMs / 1000;
                    rec.Watches++;
                }
                else
                {
                    // find the relevant movie record and make a note of some data from it
                    movieFound = false;
                    foreach(var movie in movies)
                    {
                        if (movie.MovieId == i.MovieId)
                        {
                            title = movie.Title;
                            releaseYear = movie.ReleaseYear;
                            movieFound = true;
                            break;
                        }
                    }

                    // create a new stats record with data from movie record and instance 
                    if (movieFound)
                    {
                        rec = new MovieStats()
                        {
                            MovieId = i.MovieId,
                            Title = title,
                            AverageWatchDurationS = i.WatchDurationMs / 1000,
                            Watches = 1,
                            ReleaseYear = releaseYear,
                        };
                        result.Add(i.MovieId, rec);
                    }
                }
            }

            return result.Values;
        }

    }
}