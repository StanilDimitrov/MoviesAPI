using Microsoft.EntityFrameworkCore;
using MoviesApi.Data.Models;

namespace MoviesApi.Data
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> MovieItems { get; set; }
    }
}
