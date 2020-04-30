using System;

namespace MoviesApi.Models.Movies.Response
{
    public class MovieGridResponseModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
