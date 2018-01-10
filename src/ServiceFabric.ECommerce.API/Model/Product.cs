using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ServiceFabric.ECommerce.API.Model
{
    public class Product
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        public Product()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        public Product(ProductCatalog.Model.Product p)
        {
            Id = p.Id;
            Name = p.Name;
            Description = p.Description;
            Price = p.Price;
            IsAvailable = p.Availability > 0;
        }
    }
}
