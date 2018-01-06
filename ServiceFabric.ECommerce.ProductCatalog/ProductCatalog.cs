using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceFabric.ECommerce.ProductCatalog.Model;

namespace ServiceFabric.ECommerce.ProductCatalog
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class ProductCatalog : StatefulService, IProductCatalogService
    {
        private readonly IProductRepository _repository;

        public ProductCatalog(StatefulServiceContext context)
            : base(context)
        {
            _repository = new SfProductCatalogRepository(this.StateManager);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            //return new ServiceReplicaListener[]
            //{
            //    new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context)),
            //};
            return this.CreateServiceRemotingReplicaListeners();
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            await _repository.AddProduct(
                new Product
                {
                    Id = new Guid("E5B02550-E89E-4FBE-AC07-05D8E46FE0E2"),
                    Name = "Dell XPS 1520",
                    Description = "Powerfull laptop",
                    Price = 1630,
                    Availability = 10
                }
            );

            await _repository.AddProduct(
                new Product
                {
                    Id = new Guid("{85B85730-4091-44AC-886E-E60166B323AA}"),
                    Name = "WASD Keyboard",
                    Description = "Mechanical Keyboard",
                    Price = 150,
                    Availability = 20
                }
            );
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _repository.GetAllProducts();
        }

        public async Task AddProduct(Product product)
        {
            await _repository.AddProduct(product);
        }

        public async Task<Product> GetProduct(Guid productId)
        {
            return await _repository.GetProduct(productId);
        }
    }
}
