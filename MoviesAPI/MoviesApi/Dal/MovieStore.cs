using EntityFrameworkPaginate;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Dal.Contracts;
using MoviesApi.Dal.Data.Models;
using MoviesApi.Models.Movies.Request;
using MoviesApi.Models.Movies.Response;
using MoviesApi.Models.Query;
using System;
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
          
            var movie = await _context
                .MovieItems
                .FirstOrDefaultAsync(x => x.Title == movieModel.Title, cancellationToken);

            if (movie == null)
            {
                movie = new Movie() { Title = movieModel.Title, Description = movieModel.Description, ReleaseDate = movieModel.ReleaseDate };

            }

            _context.MovieItems.Add(movie);
            await _context.SaveChangesAsync(cancellationToken);

            return movie.Id;
        }

        public async Task UpdateMovieAsync(int id, MovieUpdateRequestModel request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var movie = await _context.MovieItems.FindAsync(id);

            if (movie == null)
            {
                throw new ArgumentException($"Cannot update movie with Id: {id}.");
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
                throw new ArgumentException($"Cannot delete movie with Id: {id}.");
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
                throw new ArgumentException($"Cannot get movie details for id: {id}.");
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
                if (filter.FieldName == "Title")
                {
                    filters.Add(!string.IsNullOrEmpty(filter.Value), x => x.Title.Contains(filter.Value));
                }
                if (filter.FieldName == "ReleaseDate")
                {
                    filters.Add(!string.IsNullOrEmpty(filter.Value), x => x.ReleaseDate.Equals(filter.Value));
                }
            }
            
            return filters;
        }

        private Sorts<Movie> ApplySorting(SortModel requestSort)
        {
            var sorts = new Sorts<Movie>();
            sorts.Add(requestSort.SortBy == 1, x => x.Id);
            sorts.Add(requestSort.SortBy == 2, x => x.Title);
            sorts.Add(requestSort.SortBy == 3, x => x.ReleaseDate);

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
