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
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieStore _movieStore;
        private readonly ILogger<MoviesController> _logger;
        private readonly IMemoryCache _cache;

        public MoviesController(IMovieStore movieStore, ILogger<MoviesController> logger, IMemoryCache cache )
        {
            _movieStore = movieStore ?? throw new ArgumentNullException(nameof(movieStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        // GET: api/Movies/
        [HttpGet]
        public async Task<ActionResult<QueryResult<MovieGridResponseModel>>> GetMovieGridAsync(BasicQuery query, CancellationToken cancellationToken)
        { 
            _logger.LogInformation("Call made to GetMovieGrid.");
            return await _movieStore.GetMovieGridAsync(query, cancellationToken);
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDetailsResponseModel>> GetMovieDetailsAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to G" +
                "etMovieDetailsAsync.");

            try
            {
                return await SetGetMemoryCache(id, cancellationToken);
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

        // POST: api/Movies
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<int>> CreateMovieAsync(MovieCreateRequestModel request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to CreateMovieAsync.");
            var response = await _movieStore.CreateMovieAsync(request, cancellationToken);

            return response;
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMovieAsync(int id, MovieUpdateRequestModel request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to UpdateMovieAsync.");
            try
            {
                await _movieStore.UpdateMovieAsync(id, request, cancellationToken);
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

            return Ok();
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovieAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to DeleteMovieAsync.");
            try
            {
                await _movieStore.DeleteMovieAsync(id, cancellationToken);

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

            return Ok();
        }

        private async Task<MovieDetailsResponseModel> SetGetMemoryCache(int id, CancellationToken cancellationToken)
        {
            //2
            string key = "MyMemoryKey-Cache";
            MovieDetailsResponseModel response;
            //If the data is present in cache the 
            //Condition will be true else it is false 
            if (!_cache.TryGetValue(key, out response))
            {
                //4.fetch the data from the object
                response = await _movieStore.GetMovieDetailsAsync(id, cancellationToken);
                //5.Save the received data in cache
                _cache.Set(key, response,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
            }
            else
            {
                response = _cache.Get(key) as MovieDetailsResponseModel;
                
            }
            return response;
        }
    }
}
