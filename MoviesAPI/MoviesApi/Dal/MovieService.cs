using MoviesApi.CustomExceptions;
using MoviesApi.Dal.Contracts;
using MoviesApi.Dal.Data.Models;
using MoviesApi.Models.Movies.Request;
using MoviesApi.Models.Movies.Response;
using MoviesApi.Models.Query;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MoviesApi.Dal
{
    public class MovieService: IMovieService
    {
        private readonly MovieContext _context;

        public MovieService(MovieContext context)
        { 
          _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc />
        public async Task<int> CreateMovieAsync(MovieRequestModel movieModel, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
          
            var movie = new Movie() { Title = movieModel.Title, Description = movieModel.Description, ReleaseDate = movieModel.ReleaseDate };

            _context.MovieItems.Add(movie);
            await _context.SaveChangesAsync(cancellationToken);

            return movie.Id;
        }

        /// <inheritdoc />
        public async Task UpdateMovieAsync(int id, MovieRequestModel request, CancellationToken cancellationToken)
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

        /// <inheritdoc />
        public async Task<QueryResult<MovieResponseModel>> GetMovieGridAsync(BasicQuery request, CancellationToken cancellationToken)
        {
            var query = _context.MovieItems.Select(
                x => new MovieResponseModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ReleaseDate = x.ReleaseDate
                }).AsQueryable();

            query = ApplyFilters(request.Filters, query);
            var totalCount = await query.CountAsync(cancellationToken);

            query = ApplySorting(request.Sort, query);
            query = ApplyPagination(request.Paging, query);
            var data = await query.ToListAsync(cancellationToken);

            var queryResult = new QueryResult<MovieResponseModel>
            {
                TotalCount = totalCount,
                Data = data,
                TotalPages = CountTotalPages(totalCount, request.Paging.PageSize)
            };

            return queryResult;
        }

        /// <inheritdoc />
        public async Task<MovieResponseModel> GetMovieDetailsAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var movie = await _context.MovieItems.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (movie == null)
            {
                throw new NotFoundException($"Movie with Id: {id} does not exist.");
            }

            var movieDetails = new MovieResponseModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate
            };

            return movieDetails;
        }

        /// <inheritdoc />
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

        private IQueryable<MovieResponseModel> ApplyFilters(IEnumerable<FilterModel> requestFilters, IQueryable<MovieResponseModel> query)
        {
            foreach (var filter in requestFilters)
            {
                if (filter.Field.ToLower() == EntityColumnNamesConstants.Title.ToLower() && filter.Value != null)
                {
                    query = query.Where(x => x.Title.ToLower().Contains(filter.Value.ToLower()));
                }
                if (filter.Field.ToLower() == EntityColumnNamesConstants.ReleaseDate.ToLower() && filter.Value != null)
                {
                    query = query.Where(x => x.ReleaseDate.Year.ToString().Equals(filter.Value));
                }
            }

            return query;
        }

        private IQueryable<MovieResponseModel> ApplySorting(SortModel sort, IQueryable<MovieResponseModel> query)
        {
            if (sort.IsDescending)
            {
                if (sort.Field.ToLower() == EntityColumnNamesConstants.Id.ToLower())
                {
                    query = query.OrderByDescending(x => x.Id);
                }
                else if (sort.Field.ToLower() == EntityColumnNamesConstants.Title.ToLower())
                {
                    query = query.OrderByDescending(x => x.Title);
                }
                else if (sort.Field.ToLower() == EntityColumnNamesConstants.ReleaseDate.ToLower())
                {
                    query = query.OrderByDescending(x => x.ReleaseDate);
                }
            }

            else 
            {
                if (sort.Field.ToLower() == EntityColumnNamesConstants.Id.ToLower())
                {
                    query = query.OrderBy(x => x.Id);
                }

                else if (sort.Field.ToLower() == EntityColumnNamesConstants.Title.ToLower())
                {
                    query = query.OrderBy(x => x.Title);
                }
                else if (sort.Field.ToLower() == EntityColumnNamesConstants.ReleaseDate.ToLower())
                {
                    query = query.OrderBy(x => x.ReleaseDate);
                }
            }

            return query;
        }

        private IQueryable<MovieResponseModel> ApplyPagination(PagingModel pageModel, IQueryable<MovieResponseModel> query)
        {
           return query.Skip((pageModel.CurrentPage - 1) * pageModel.PageSize).Take(pageModel.PageSize);
        }

        private void SetMovieProperties(Movie movie, MovieRequestModel requestModel)
        {
            movie.Title = requestModel.Title;
            movie.Description = requestModel.Description;
            movie.ReleaseDate = requestModel.ReleaseDate;
        }

        private int CountTotalPages(int totalCount, int pageSize)
        {
            int totalPageCount = totalCount / pageSize;

            if (totalPageCount < 1)
            {
                totalPageCount = 1;
            }

            return totalPageCount;
        }
    }
}
