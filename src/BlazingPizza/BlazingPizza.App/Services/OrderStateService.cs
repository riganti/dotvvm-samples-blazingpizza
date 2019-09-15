using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazingPizza.App.Services
{
    public class OrderStateService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        // TODO: use more clever storage mechanism
        private readonly ConcurrentDictionary<Guid, Order> store = new ConcurrentDictionary<Guid, Order>();

        public OrderStateService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Order LoadCurrentOrderState()
        {
            var stateId = GetOrCreateStateId();
            return store.TryGetValue(stateId, out var result) ? result : null;
        }

        public void SaveCurrentOrderState(Order order)
        {
            var stateId = GetOrCreateStateId();
            store[stateId] = order;
        }

        private Guid GetOrCreateStateId()
        {
            string stateId;
            if (!httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("OrderState", out stateId))
            {
                stateId = Guid.NewGuid().ToString();
                httpContextAccessor.HttpContext.Response.Cookies.Append("OrderState", stateId);
            }
            return new Guid(stateId);
        }

    }
}
