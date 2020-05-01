using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MoviesApi.CustomExceptions;
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
        private readonly IMovieStore _movieStore;
        private readonly IMemoryCache _cache;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor for MoviesController
        /// </summary>
        /// <param name="movieStore">Provides access to CRUD operations on SecurityProfile</param>                
        /// <param name="cache">Provides access to use memory cache</param>                
        /// <param name="logger">Provides logging services</param> 
        public MoviesController(IMovieStore movieStore, ILogger<MoviesController> logger, IMemoryCache cache )
        {
            _movieStore = movieStore ?? throw new ArgumentNullException(nameof(movieStore));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get list of movies action
        /// </summary>
        /// <param name="request">The request model</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>Query result instance containing total count, total page count and the result set.</returns>
        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<QueryResult<MovieGridResponseModel>>> GetMovieGridAsync(BasicQuery request, CancellationToken cancellationToken)
        { 
            _logger.LogInformation("Call made to GetMovieGrid.");
            return await _movieStore.GetMovieGridAsync(request, cancellationToken);
        }

        /// <summary>
        /// Get movie details action
        /// </summary>
        /// <param name="id">The movie unique identifier</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>Movie details response model</returns>
        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDetailsResponseModel>> GetMovieDetailsAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to G" +
                "etMovieDetailsAsync.");

            try
            {
                return await CacheDetailsResponse(id, cancellationToken);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected exception occured:", ex);
                throw;
            }
        }

        /// <summary>
        /// Create movie action
        /// </summary>
        /// <param name="MovieCreateRequestModel">Input parameters for creation of movie wrapped in an object</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>The new movie unique identifier</returns>
        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<int>> CreateMovieAsync(MovieCreateRequestModel request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to CreateMovieAsync.");
            return await _movieStore.CreateMovieAsync(request, cancellationToken);
        }

        /// <summary>
        /// Update movie action
        /// </summary>
        /// <param name="MovieUpdateRequestModel">Input parameters for updating of movie profile wrapped in an object</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// /// <returns>The task</returns>
        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMovieAsync(int id, MovieUpdateRequestModel request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to UpdateMovieAsync.");
            try
            {
                await _movieStore.UpdateMovieAsync(id, request, cancellationToken);
                return Ok();
            }
            catch (NotFoundException ex)
            {

                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected exception occured:", ex);
                throw;
            }
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
            try
            {
                await _movieStore.DeleteMovieAsync(id, cancellationToken);
                return Ok();

            }
            catch (NotFoundException ex)
            {

                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError("Unexpected exception occured:", ex);
                throw;
            }
        }

        private async Task<MovieDetailsResponseModel> CacheDetailsResponse(int id, CancellationToken cancellationToken)
        {
            var cashedResponse = await _cache.GetOrCreateAsync<MovieDetailsResponseModel>("DetailsResponse", async (cacheEntry) =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(1);
                var response = await _movieStore.GetMovieDetailsAsync(id, cancellationToken);
                return response;
            });

            return cashedResponse;
        }
    }
}
