using System;

namespace MoviesApi.Models.Movies.Response
{
    /// </summary>
    /// This response model is used to get the information about a movie.
    /// </summary>
    public class MovieResponseModel
    {
        /// <summary>
        /// Gets or sets the movie unique identifier.
        /// </summary>
        /// <value>
        /// The movie unique identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the movie title.
        /// </summary>
        /// <value>
        /// The movie title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the movie description.
        /// </summary>
        /// <value>
        /// The movie description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the release date.
        /// </summary>
        /// <value>
        /// The release date.
        /// </value>
        public DateTime ReleaseDate { get; set; }
    }
}
