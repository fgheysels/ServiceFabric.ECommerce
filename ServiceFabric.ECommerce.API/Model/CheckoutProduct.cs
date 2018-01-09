using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ServiceFabric.ECommerce.API.Model
{
    public class CheckoutProduct
    {
        [JsonProperty("productId")]
        public Guid ProductId { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutProduct"/> class.
        /// </summary>
        public CheckoutProduct(CheckoutService.Model.CheckoutProduct product )
        {
            this.ProductId = product.Product.Id;
            this.ProductName = product.Product.Name;
            this.Quantity = product.Quantity;
            this.Price = product.Price;
        }
    }
}
