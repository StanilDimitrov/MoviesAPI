using Microsoft.EntityFrameworkCore;
using MoviesApi.CustomExceptions;
using MoviesApi.Dal.Contracts;
using MoviesApi.Dal.Data.Models;
using MoviesApi.Dal.Mappers;
using MoviesApi.Models.Movies.Request;
using MoviesApi.Models.Movies.Response;
using MoviesApi.Models.Query;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<QueryResult<MovieGridResponseModel>> GetMovieGridAsync(BasicQuery request, CancellationToken cancellationToken)
        {
            var query = _context.MovieItems.Select(x => new MovieGridResponseModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ReleaseDate = x.ReleaseDate
            }).AsQueryable();
            
            query = ApplyFilters(request.Filters, query);
            var totalCount = await query.CountAsync(cancellationToken);

            List<MovieGridResponseModel> data = new List<MovieGridResponseModel>();
            query = ApplySorting(request.Sort, query);
            query = ApplyPagination(request.Paging, query);
            data = await query.ToListAsync(cancellationToken);

            QueryResult<MovieGridResponseModel> queryResult = new QueryResult<MovieGridResponseModel> { TotalCount = totalCount, Data = data, TotalPages = totalCount / request.Paging.PageSize};
            
            return queryResult;
        }

        private IQueryable<MovieGridResponseModel> ApplyFilters(IEnumerable<FilterModel> requestFilters, IQueryable<MovieGridResponseModel> query)
        {
            foreach (var filter in requestFilters)
            {
                if (filter.Field == "Title" && filter.Value != null)
                {
                    query = query.Where(x => x.Title.ToLower().Contains(filter.Value.ToLower()));
                }
                if (filter.Field == "ReleaseDate" && filter.Value != null)
                {
                    query = query.Where(x => x.ReleaseDate.Year.ToString().Equals(filter.Value));
                }
            }

            return query;
        }

        private IQueryable<MovieGridResponseModel> ApplySorting(SortModel sort, IQueryable<MovieGridResponseModel> query)
        {
            if (sort.IsDescending)
            {
                if (sort.Field == "Id")
                {
                    query = query.OrderByDescending(x => x.Id);
                }
                else if (sort.Field == "Title")
                {
                    query = query.OrderByDescending(x => x.Title);
                }
                else if (sort.Field == "ReleaseDate")
                {
                    query = query.OrderByDescending(x => x.ReleaseDate);
                }
            }

            else 
            {
                if (sort.Field == "Id")
                {
                    query = query.OrderBy(x => x.Id);
                }

                else if (sort.Field == "Title")
                {
                    query = query.OrderBy(x => x.Title);
                }
                else if (sort.Field == "ReleaseDate")
                {
                    query = query.OrderBy(x => x.ReleaseDate);
                }
            }

            return query;
        }

        private IQueryable<MovieGridResponseModel> ApplyPagination(PagingModel pageModel, IQueryable<MovieGridResponseModel> query)
        {
           return query.Skip((pageModel.CurrentPage - 1) * pageModel.PageSize).Take(pageModel.PageSize);
        }

        private void SetMovieProperties(Movie movie, MovieUpdateRequestModel requestModel)
        {
            movie.Title = requestModel.Title;
            movie.Description = requestModel.Description;
            movie.ReleaseDate = requestModel.ReleaseDate;
        }
    }
}
