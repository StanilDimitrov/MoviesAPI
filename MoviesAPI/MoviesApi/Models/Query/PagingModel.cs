using Newtonsoft.Json;

namespace MoviesApi.Models.Query
{
    /// <summary>
    /// Gets or sets the offset of the query.
    /// </summary>
    /// <remarks>
    /// This represent how many rows to skip from the result set.
    /// </remarks>
    public class PagingModel
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }
    }
}
