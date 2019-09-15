using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlazingPizza.App.Services;
using DotVVM.Framework.ViewModel;

namespace BlazingPizza.App.ViewModels
{
    public class CheckoutViewModel : MasterPageViewModel
    {
        private readonly HttpClient httpClient;
        private readonly OrderStateService orderStateService;

        public CheckoutViewModel(HttpClient httpClient, OrderStateService orderStateService)
        {
            this.httpClient = httpClient;
            this.orderStateService = orderStateService;
        }

        public Order Order { get; set; }

        public override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                Order = orderStateService.LoadCurrentOrderState();
                if (Order == null)
                {
                    Context.RedirectToRoute("GetPizza");
                }
            }

            return base.PreRender();
        }

        public async Task PlaceOrder()
        {
            var newOrderId = await httpClient.PostJsonAsync<int>("orders", Order);
            orderStateService.SaveCurrentOrderState(new Order());
            Context.RedirectToRoute("OrderDetails", new { Id = newOrderId });
        }
    }
}

