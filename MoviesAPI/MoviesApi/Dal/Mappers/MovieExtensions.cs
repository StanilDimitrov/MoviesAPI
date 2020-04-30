using MoviesApi.Dal.Data.Models;
using MoviesApi.Models.Movies.Response;

namespace MoviesApi.Dal.Mappers
{
    public static class MovieExtensions
    {
        public static MovieGridResponseModel Map (this Movie entity)
        {
            if (entity is null)
            {
                return null;
            }

            return new MovieGridResponseModel()
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                ReleaseDate = entity.ReleaseDate
            };
        }
    }
}
