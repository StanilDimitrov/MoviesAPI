using MoviesApi.Models.Movies.Request;
using MoviesApi.Models.Movies.Response;
using MoviesApi.Models.Query;
using System.Threading;
using System.Threading.Tasks;

namespace MoviesApi.Dal.Contracts
{
    public interface IMovieStore
    {
        /// <summary>
        /// Create movie asynchronously.
        /// </summary>
        /// <param name="request">The request model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task</returns>
        Task<int> CreateMovieAsync(MovieCreateRequestModel request, CancellationToken cancellationToken);

        /// <summary>
        /// Update movie asynchronously.
        /// </summary>
        /// <param name="id">The movie unique identifier.</param>
        /// <param name="request">The request model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task</returns>
        Task UpdateMovieAsync(int id, MovieUpdateRequestModel request, CancellationToken cancellationToken);

        /// <summary>
        /// Delete movie asynchronously.
        /// </summary>
        /// <param name="id">The movie unique identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task</returns>
        Task DeleteMovieAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Get movie details asynchronously.
        /// </summary>
        /// <param name="id">The movie unique identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The movie details response model</returns>
        Task<MovieDetailsResponseModel> GetMovieDetailsAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Get movie grid asynchronously.
        /// </summary>
        /// <param name="request">The request model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Query result instance containing total count, total page count and the result set.</returns>
        Task<QueryResult<MovieGridResponseModel>> GetMovieGridAsync(BasicQuery request, CancellationToken cancellationToken);

    }
}
