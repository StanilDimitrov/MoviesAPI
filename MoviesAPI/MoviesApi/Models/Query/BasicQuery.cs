using System.Collections.Generic;

namespace MoviesApi.Models.Query
{
    public class BasicQuery
    {
        public PagingModel Paging { get; set; }

        public SortModel Sort { get; set; }

        public IEnumerable<FilterModel> Filters { get; set; }
    }
}
