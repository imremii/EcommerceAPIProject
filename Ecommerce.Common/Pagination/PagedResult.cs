using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public PaginationMetadata Metadata { get; set; } = new();
    }
}
