using EntityFrameworkPaginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoviesApi.Models.Query
{
    public class BasicQuery
    {
        public PagingModel Paging { get; set; }

        public SortModel Sort { get; set; }

        public IEnumerable<FilterModel> Filters { get; set; }

    }
}
