using EntityFrameworkPaginate;
using Microsoft.EntityFrameworkCore;
using MoviesApi.CustomExceptions;
using MoviesApi.Dal.Contracts;
using MoviesApi.Dal.Data.Models;
using MoviesApi.Models.Movies.Request;
using MoviesApi.Models.Movies.Response;
using MoviesApi.Models.Query;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoviesApi.Dal
{
    public class MovieStore: IMovieStore
    {
        private readonly MovieContext _context;

        public MovieStore(MovieContext context)
        {
            _context = context;
        }

        public async Task<int> CreateMovieAsync(MovieCreateRequestModel movieModel, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
          
            var movie = new Movie() { Title = movieModel.Title, Description = movieModel.Description, ReleaseDate = movieModel.ReleaseDate };

            _context.MovieItems.Add(movie);
            await _context.SaveChangesAsync(cancellationToken);

            return movie.Id;
        }

        public async Task UpdateMovieAsync(int id, MovieUpdateRequestModel request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var movie = await _context.MovieItems.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (movie == null)
            {
                throw new NotFoundException($"Movie with Id: {id} does not exist.");
            }

            SetMovieProperties(movie, request);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteMovieAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var movie = await _context.MovieItems.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (movie == null)
            {
                throw new NotFoundException($"Movie with Id: {id} does not exist.");
            }

            _context.MovieItems.Remove(movie);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<MovieDetailsResponseModel> GetMovieDetailsAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var movie = await _context.MovieItems.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (movie == null)
            {
                throw new NotFoundException($"Movie with Id: {id} does not exist.");
            }

            var movieDetails = new MovieDetailsResponseModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate
            };
         
            return movieDetails;
        }

        public Page<Movie> GetMovieGridAsync(BasicQuery query)
        {
            var filters = ApplyFilters(query.Filters);
            var sorts = ApplySorting(query.Sort);

            var data = _context.MovieItems.Paginate(query.Paging.CurrentPage, query.Paging.PageSize, sorts, filters);
            
            return data;
        }

        private Filters<Movie> ApplyFilters(IEnumerable<FilterModel> requestFilters)
        {
            var filters = new Filters<Movie>();
            foreach (var filter in requestFilters)
            {
                if (filter.Field == "Title")
                {
                    filters.Add(!string.IsNullOrEmpty(filter.Value), x => x.Title.Contains(filter.Value));
                }
                if (filter.Field == "ReleaseDate")
                {
                    filters.Add(!string.IsNullOrEmpty(filter.Value), x => x.ReleaseDate.Year.ToString().Equals(filter.Value));
                }
            }
            
            return filters;
        }

        private Sorts<Movie> ApplySorting(SortModel requestSort)
        {
            var sorts = new Sorts<Movie>();
            
            sorts.Add(requestSort.Field == "Id", x => x.Id);
            sorts.Add(requestSort.Field == "Title", x => x.Title);
            sorts.Add(requestSort.Field == "ReleaseDate", x => x.ReleaseDate);

            return sorts;
        }

        private void SetMovieProperties(Movie movie, MovieUpdateRequestModel requestModel)
        {
            movie.Title = requestModel.Title;
            movie.Description = requestModel.Description;
            movie.ReleaseDate = requestModel.ReleaseDate;
        }
    }
}
