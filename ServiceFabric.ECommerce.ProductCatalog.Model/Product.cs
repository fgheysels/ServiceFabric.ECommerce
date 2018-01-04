using System;
using System.Linq;
using System.Text;

namespace ServiceFabric.ECommerce.ProductCatalog.Model
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public int Availability { get; set; }
    }
}
