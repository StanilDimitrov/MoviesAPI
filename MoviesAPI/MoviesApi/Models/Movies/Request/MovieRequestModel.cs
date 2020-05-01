using System;

namespace MoviesApi.Models.Movies.Request
{
    /// <summary>
    /// Request model for movie properties
    /// </summary>
    public class MovieRequestModel
    {
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
