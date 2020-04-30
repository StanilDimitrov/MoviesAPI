using EntityFrameworkPaginate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesApi.Dal.Contracts;
using MoviesApi.Dal.Data.Models;
using MoviesApi.Models.Movies.Request;
using MoviesApi.Models.Movies.Response;
using MoviesApi.Models.Query;
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

        public MoviesController(IMovieStore movieStore, ILogger<MoviesController> logger)
        {
            _movieStore = movieStore;
            _logger = logger;
        }

        // GET: api/Movies
        [HttpGet]
        public ActionResult<Page<Movie>> GetMovieGrid(BasicQuery query)
        {
            _logger.LogInformation("Call made to GetMovieGrid.");
            var data = _movieStore.GetMovieGridAsync(query);

            return data;
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDetailsResponseModel>> GetMovieDetailsAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to GetMovieDetailsAsync.");
            var response = await _movieStore.GetMovieDetailsAsync(id, cancellationToken);
            return response;
        }

        // POST: api/Movies
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<int>> CreateMovieAsync(MovieCreateRequestModel request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to CreateMovieAsync.");
            return await _movieStore.CreateMovieAsync(request, cancellationToken);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMovieAsync(int id, MovieUpdateRequestModel request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to UpdateMovieAsync.");
            await _movieStore.UpdateMovieAsync(id, request, cancellationToken);

            return Ok();
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovieAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call made to DeleteMovieAsync.");
            await _movieStore.DeleteMovieAsync(id, cancellationToken);

            return Ok();
        }

    }
}
