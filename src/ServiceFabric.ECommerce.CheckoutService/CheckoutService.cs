using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceFabric.ECommerce.CheckoutService.Model;
using ServiceFabric.ECommerce.ProductCatalog.Model;
using UserActor.Interfaces;

namespace ServiceFabric.ECommerce.CheckoutService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class CheckoutService : StatefulService, ICheckoutService
    {
        public CheckoutService(StatefulServiceContext context)
            : base(context) { }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();
        }

        public async Task<CheckoutSummary> Checkout(string userId)
        {
            var result = new CheckoutSummary();
            result.Date = DateTime.Now;
            result.Products = new List<CheckoutProduct>();

            // Call the UserActor for the current user and get his shopping cart.
            var userActor = GetUserActor(userId);

            var cartContent = await userActor.GetCart();

            var catalogService = GetProductCatalogService();

            // Retrieve all product information in one call.
            var productInformation = await catalogService.GetProducts(cartContent.Keys);

            foreach (var item in cartContent)
            {
                if (productInformation.ContainsKey(item.Key) == false)
                {
                    continue;
                }

                var product = productInformation[item.Key];

                var checkoutProduct = new CheckoutProduct()
                {
                    Product = product,
                    Price = product.Price,
                    Quantity = item.Value
                };

                result.Products.Add(checkoutProduct);
            }

            // Add the checkout to the history.
            await userActor.AddCheckoutInformationToHistory(result);            

            await userActor.ClearCart();

            return result;
        }

        private static IUserActor GetUserActor(string userId)
        {
            return ActorProxy.Create<IUserActor>(new ActorId(userId), new Uri("fabric:/ServiceFabric.ECommerce/UserActorService"));
        }

        private static IProductCatalogService GetProductCatalogService()
        {
            return ServiceProxy.Create<IProductCatalogService>(new Uri("fabric:/ServiceFabric.ECommerce/ServiceFabric.ECommerce.ProductCatalog"), new ServicePartitionKey(0));
        }

        public async Task<IEnumerable<CheckoutSummary>> GetOrderHistory(string userId)
        {
            var userActor = GetUserActor(userId);
            return await userActor.GetCheckoutHistory();
        }
    }
}
