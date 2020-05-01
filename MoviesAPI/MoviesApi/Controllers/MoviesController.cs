using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MoviesApi.Dal.Contracts;
using MoviesApi.Models.Movies.Request;
using MoviesApi.Models.Movies.Response;
using MoviesApi.Models.Query;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MoviesApi.Controllers
{
    /// <summary>
    /// A class that redirects user's request to specific movie action.
    /// It inherits class ControllerBase.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IMemoryCache _cache;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor for MoviesController
        /// </summary>
        /// <param name="movieStore">Provides access to CRUD operations on SecurityProfile</param>                
        /// <param name="cache">Provides access to use memory cache</param>                
        /// <param name="logger">Provides logging services</param> 
        public MoviesController(IMovieService movieStore, ILogger<MoviesController> logger, IMemoryCache cache)
        {
            _movieService = movieStore ?? throw new ArgumentNullException(nameof(movieStore));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Create movie action
        /// </summary>
        /// <param name="MovieRequestModel">Input parameters for creation of movie wrapped in an object</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>The new movie unique identifier</returns>
        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<int>> CreateMovieAsync(MovieRequestModel request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to CreateMovieAsync.");
            return await _movieService.CreateMovieAsync(request, cancellationToken);
        }

        /// <summary>
        /// Update movie action
        /// </summary>
        /// <param name="MovieRequestModel">Input parameters for updating the movie  wrapped in an object</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// /// <returns>The task</returns>
        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMovieAsync(int id, MovieRequestModel request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to UpdateMovieAsync.");

            await _movieService.UpdateMovieAsync(id, request, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Get list of movies action
        /// </summary>
        /// <param name="request">The request model</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>Query result instance containing total count, total page count and the result set.</returns>
        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<QueryResult<MovieResponseModel>>> GetMovieGridAsync(BasicQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to GetMovieGrid.");
            return await _movieService.GetMovieGridAsync(request, cancellationToken);
        }

        /// <summary>
        /// Get movie details action
        /// </summary>
        /// <param name="id">The movie unique identifier</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>Movie response model</returns>
        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieResponseModel>> GetMovieDetailsAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to GetMovieDetailsAsync");
               
            return await CacheDetailsResponse(id, cancellationToken);
        }

        /// <summary>
        /// Delete movie action
        /// </summary>
        /// <param name="id">The movie unique identifier</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// /// <returns>The task</returns>
        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovieAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to DeleteMovieAsync.");

            await _movieService.DeleteMovieAsync(id, cancellationToken);
            return Ok();
        }

        private async Task<MovieResponseModel> CacheDetailsResponse(int id, CancellationToken cancellationToken)
        {
            var cashedResponse = await _cache.GetOrCreateAsync<MovieResponseModel>("DetailsResponse", async (cacheEntry) =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(1);
                var response = await _movieService.GetMovieDetailsAsync(id, cancellationToken);
                return response;
            });

            return cashedResponse;
        }
    }
}
