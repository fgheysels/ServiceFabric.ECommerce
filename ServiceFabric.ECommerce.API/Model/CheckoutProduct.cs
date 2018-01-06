using System;
using System.Collections.Generic;
using System.Linq;
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

    public class CheckoutSummary
    {
        [JsonProperty("products")]
        public List<CheckoutProduct> Products { get; set; }

        [JsonProperty("totalprice")]
        public decimal TotalPrice { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutSummary"/> class.
        /// </summary>
        public CheckoutSummary ( CheckoutService.Model.CheckoutSummary summary)
        {
            this.TotalPrice = summary.TotalPrice;
            this.Date = summary.Date;
            Products = new List<CheckoutProduct>(summary.Products.Select(p => new CheckoutProduct(p)));
        }

    }
}
