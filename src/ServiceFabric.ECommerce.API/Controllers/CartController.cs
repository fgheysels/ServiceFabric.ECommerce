using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using ServiceFabric.ECommerce.API.Model;
using UserActor.Interfaces;

namespace ServiceFabric.ECommerce.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Cart")]
    public class CartController : Controller
    {
        [HttpGet("{userId}")]
        public async Task<Cart> Get(string userId)
        {
            var actor = GetActor(userId);

            var cartContents = await actor.GetCart();

            return new Cart()
            {
                UserId = userId,
                Items = cartContents.Select(p => new CartItem() {ProductId = p.Key.ToString(), Quantity = p.Value}).ToArray()
            };
        }

        [HttpPost("{userId}")]
        public async Task Add(string userId, [FromBody] AddToCartRequest request)
        {
            var actor = GetActor(userId);

            await actor.AddToCart(request.ProductId, request.Quantity);
        }

        [HttpDelete("{userId}")]
        public async Task Delete(string userId)
        {
            var actor = GetActor(userId);
            await actor.ClearCart();
        }

        private static IUserActor GetActor(string userId)
        {
            return ActorProxy.Create<IUserActor>(new ActorId(userId), 
                                                 new Uri("fabric:/ServiceFabric.ECommerce/UserActorService"));
        }
    }
}