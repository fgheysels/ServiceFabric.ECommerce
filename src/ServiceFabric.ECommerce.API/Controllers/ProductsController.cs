using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using ServiceFabric.ECommerce.ProductCatalog.Model;
using Product = ServiceFabric.ECommerce.API.Model.Product;

namespace ServiceFabric.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductCatalogService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        public ProductsController()
        {
            _service =
                ServiceProxy.Create<IProductCatalogService>(new Uri("fabric:/ServiceFabric.ECommerce/ServiceFabric.ECommerce.ProductCatalog"),
                                                            new ServicePartitionKey(0));
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            var products = await _service.GetAllProducts();

            return products.Select(p => new Product(p));
        }

        [HttpPost]
        public async Task Post([FromBody]Product product)
        {
            var catalogProduct = new ProductCatalog.Model.Product();

            catalogProduct.Id = product.Id;
            catalogProduct.Name = product.Name;
            catalogProduct.Description = product.Description;
            catalogProduct.Availability = 100;
            catalogProduct.Price = product.Price;

            await _service.AddProduct(catalogProduct);
        }

    }
}
