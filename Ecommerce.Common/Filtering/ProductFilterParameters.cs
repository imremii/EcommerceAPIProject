using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Common
{
    public class ProductFilterParameters : BaseFilterParameters
    {
        public int MinCount { get; set; }
        public int MaxCount { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
