using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ServiceFabric.ECommerce.API.Model {
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