using MoviesApi.Models.Movies.Request;
using MoviesApi.Models.Movies.Response;
using MoviesApi.Models.Query;
using System.Threading;
using System.Threading.Tasks;

namespace MoviesApi.Dal.Contracts
{
    public interface IMovieStore
    {
        Task<int> CreateMovieAsync(MovieCreateRequestModel movieModel, CancellationToken cancellationToken);

        Task UpdateMovieAsync(int id, MovieUpdateRequestModel request, CancellationToken cancellationToken);

        Task DeleteMovieAsync(int id, CancellationToken cancellationToken);

        Task<MovieDetailsResponseModel> GetMovieDetailsAsync(int id, CancellationToken cancellationToken);

        Task<QueryResult<MovieGridResponseModel>> GetMovieGridAsync(BasicQuery request, CancellationToken cancellationToken);

    }
}
