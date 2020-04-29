using Microsoft.EntityFrameworkCore;
using MoviesApi.Dal.Data.Models;

namespace MoviesApi.Dal
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
