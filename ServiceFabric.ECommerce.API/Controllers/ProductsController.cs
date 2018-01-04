using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceFabric.ECommerce.API.Model;

namespace ServiceFabric.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return new[] { new Product() { Id = Guid.NewGuid(), Name = "Fake Product", Description = "Fake Product" } };
        }

        [HttpPost]
        public void Post([FromBody]Product product)
        {
        }

    }
}
