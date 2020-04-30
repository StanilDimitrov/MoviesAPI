using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoviesApi.Dal.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoviesApi.Dal
{
    public static class DataSeed
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MovieContext(
                serviceProvider.GetRequiredService<DbContextOptions<MovieContext>>()))
            {
                // Look for any movies.
                if (context.MovieItems.Any())
                {
                    return;   // Data was already seeded
                }

                var movies = new List<Movie>()
                {
                    DataSeedHelper.TestMovie1(),
                    DataSeedHelper.TestMovie2(),
                    DataSeedHelper.TestMovie3(),
                    DataSeedHelper.TestMovie4()
                };

                context.MovieItems.AddRange(movies);
                context.SaveChanges();
            }
        }
    }
}
