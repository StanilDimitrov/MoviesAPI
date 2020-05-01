using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Data.Models
{
    /// <summary>
    /// Entity model for Movie table
    /// </summary>
    public class Movie
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
        [StringLength(250)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the movie description.
        /// </summary>
        /// <value>
        /// The movie description.
        /// </value>
        [StringLength(500)]
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
