using System;

namespace MoviesApi.Models.Movies.Request
{
    public class MovieCreateRequestModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
