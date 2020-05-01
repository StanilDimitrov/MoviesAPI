using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoviesApi.Dal;
using MoviesApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoviesApi.Data
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

                var movies = new List<Movie>
                {
                    DataSeedHelper.TestMovie1(),
                    DataSeedHelper.TestMovie2(),
                    DataSeedHelper.TestMovie3(),
                    DataSeedHelper.TestMovie4(),
                    DataSeedHelper.TestMovie5(),
                    DataSeedHelper.TestMovie6(),
                    DataSeedHelper.TestMovie7(),
                    DataSeedHelper.TestMovie8(),
                    DataSeedHelper.TestMovie9(),
                    DataSeedHelper.TestMovie10()
                };

                context.MovieItems.AddRange(movies);
                context.SaveChanges();
            }
        }
    }
}
