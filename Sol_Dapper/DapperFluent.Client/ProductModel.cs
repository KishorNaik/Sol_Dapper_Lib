using System;
using System.Collections.Generic;
using System.Text;

namespace DapperFluent.Client
{
    public class ProductModel
    {
        public decimal? ProductId { get; set; }

        public String Name { get; set; }

        public Decimal? UnitPrice { get; set; }
    }
}