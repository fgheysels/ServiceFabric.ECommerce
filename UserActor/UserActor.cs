using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using UserActor.Interfaces;

namespace UserActor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class UserActor : Actor, IUserActor
    {
        /// <summary>
        /// Initializes a new instance of UserActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public UserActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            // The StateManager is this actor's private state store.
            // Data stored in the StateManager will be replicated for high-availability for actors that use volatile or persisted state storage.
            // Any serializable object can be saved in the StateManager.
            // For more information, see https://aka.ms/servicefabricactorsstateserialization

            return Task.CompletedTask;
        }

        public async Task AddToCart(Guid productId, int quantity)
        {
            await StateManager.AddOrUpdateStateAsync(productId.ToString(), quantity, (id, oldQty) => oldQty + quantity);
        }

        public async Task<Dictionary<Guid, int>> GetCart()
        {
            var result = new Dictionary<Guid, int>();

            var productIds = await StateManager.GetStateNamesAsync(CancellationToken.None);

            foreach (string pId in productIds)
            {
                result.Add(new Guid(pId), await StateManager.GetStateAsync<int>(pId));
            }

            return result;
        }

        public async Task ClearCart()
        {
            var productIds = await StateManager.GetStateNamesAsync(CancellationToken.None);

            foreach (string pId in productIds)
            {
                await StateManager.RemoveStateAsync(pId);
            }
        }

        //public Task AddCheckoutInformationToHistory(CheckoutSummary checkoutSummary)
        //{
        //    return Task.CompletedTask;

        //    //await StateManager.AddOrUpdateStateAsync("history",
        //    //                                         new List<CheckoutSummary>() { checkoutSummary },
        //    //                                         ((s, list) =>
        //    //                                         {
        //    //                                             list.Add(checkoutSummary);
        //    //                                             return list;
        //    //                                         }));
        //}

        //public async Task<IEnumerable<CheckoutSummary>> GetCheckoutHistory()
        //{
        //    return Task.FromResult(new );

        //    //var history = await StateManager.TryGetStateAsync<List<CheckoutSummary>>("history");

        //    //return history.HasValue ? history.Value.ToArray() : new CheckoutSummary[] { };
        //}
    }
}
