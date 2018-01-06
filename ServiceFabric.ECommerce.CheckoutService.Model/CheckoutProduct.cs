using ServiceFabric.ECommerce.ProductCatalog.Model;

namespace ServiceFabric.ECommerce.CheckoutService.Model {
    public class CheckoutProduct
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}