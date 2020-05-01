using MoviesApi.Models.Movies.Request;
using MoviesApi.Models.Movies.Response;
using MoviesApi.Models.Query;
using System.Threading;
using System.Threading.Tasks;

namespace MoviesApi.Dal.Contracts
{
    public interface IMovieService
    {
        /// <summary>
        /// Create movie asynchronously.
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The movie unique identifier</returns>
        Task<int> CreateMovieAsync(MovieRequestModel requestModel, CancellationToken cancellationToken);

        /// <summary>
        /// Update movie asynchronously.
        /// </summary>
        /// <param name="id">The movie unique identifier.</param>
        /// <param name="requestModel">The request model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task</returns>
        Task UpdateMovieAsync(int id, MovieRequestModel requestModel, CancellationToken cancellationToken);

        /// <summary>
        /// Get movie grid asynchronously.
        /// </summary>
        /// <param name="request">The request model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Query result instance containing total count, total page count and the result set.</returns>
        Task<QueryResult<MovieResponseModel>> GetMovieGridAsync(BasicQuery request, CancellationToken cancellationToken);

        /// <summary>
        /// Get movie details asynchronously.
        /// </summary>
        /// <param name="id">The movie unique identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response model</returns>
        Task<MovieResponseModel> GetMovieDetailsAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Delete movie asynchronously.
        /// </summary>
        /// <param name="id">The movie unique identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task</returns>
        Task DeleteMovieAsync(int id, CancellationToken cancellationToken);
    }
}
