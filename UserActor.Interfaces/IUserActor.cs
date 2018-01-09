using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;
using ServiceFabric.ECommerce.CheckoutService.Model;

[assembly: FabricTransportActorRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]
namespace UserActor.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IUserActor : IActor
    {        
        Task AddToCart(Guid productId, int quantity);

        Task<Dictionary<Guid, int>> GetCart();

        Task ClearCart();

        // TODO: CheckoutSummary is a class from CheckoutService.Model.
        // It would be appropriate if the UserActor has its own CheckoutSummary class I think.        
        Task AddCheckoutInformationToHistory( CheckoutSummary checkoutSummary );

        Task<List<CheckoutSummary>> GetCheckoutHistory();
    }
}
