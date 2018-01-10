using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using ServiceFabric.ECommerce.ProductCatalog.Model;

namespace ServiceFabric.ECommerce.ProductCatalog
{
    internal class SfProductCatalogRepository : IProductRepository
    {
        private readonly IReliableStateManager _stateManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SfProductCatalogRepository"/> class.
        /// </summary>
        public SfProductCatalogRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;

        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var products = await GetReliableProductsCollection();

            var list = new List<Product>();

            using (var tx = _stateManager.CreateTransaction())
            {
                var allProducts = await products.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allProducts.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        list.Add(enumerator.Current.Value);
                    }
                }
            }

            return list;
        }

        public async Task AddProduct(Product product)
        {
            IReliableDictionary<Guid, Product> products = await GetReliableProductsCollection();

            using (var tx = _stateManager.CreateTransaction())
            {
                await products.AddOrUpdateAsync(tx, product.Id, product, (id, value) => product);
                await tx.CommitAsync();
            }
        }

        public async Task<Product> GetProduct(Guid productId)
        {
            var products = await GetReliableProductsCollection();

            using (var tx = _stateManager.CreateTransaction())
            {
                var product = await products.TryGetValueAsync(tx, productId);
                await tx.CommitAsync();

                return product.HasValue ? product.Value : null;
            }
        }

        private async Task<IReliableDictionary<Guid, Product>> GetReliableProductsCollection()
        {
            var products = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");
            return products;
        }
    }
}
