using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ServiceFabric.ECommerce.API.Model
{
    public class Cart
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("item")]
        public CartItem[] Items { get; set; }
    }

    public class CartItem
    {
        [JsonProperty("productId")]
        public string ProductId { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

    public class AddToCartRequest
    {
        [JsonProperty("productId")]
        public Guid ProductId { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
