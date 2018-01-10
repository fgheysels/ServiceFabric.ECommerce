using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using ServiceFabric.ECommerce.CheckoutService.Model;
using CheckoutSummary = ServiceFabric.ECommerce.API.Model.CheckoutSummary;

namespace ServiceFabric.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class CheckoutController : Controller
    {
        [Route("{userId}")]
        public async Task<CheckoutSummary> Checkout(string userId)
        {
            var summary = await GetCheckoutService().Checkout(userId);

            return new CheckoutSummary(summary);
        }

        [Route("history/{userId}")]
        public async Task<IEnumerable<CheckoutSummary>> GetHistory(string userId)
        {
            var history = await GetCheckoutService().GetOrderHistory(userId);

            return history.Select(h => new CheckoutSummary(h));
        }

        private static ICheckoutService GetCheckoutService()
        {
            return ServiceProxy.Create<ICheckoutService>(new Uri("fabric:/ServiceFabric.ECommerce/ServiceFabric.ECommerce.CheckoutService"),
                                                         new ServicePartitionKey(0));
        }

    }
}
