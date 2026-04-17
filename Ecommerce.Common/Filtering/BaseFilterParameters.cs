using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Common
{
    public abstract class BaseFilterParameters
    {
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
    }
}
