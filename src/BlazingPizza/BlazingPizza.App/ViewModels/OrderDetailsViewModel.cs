using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;

namespace BlazingPizza.App.ViewModels
{
    public class OrderDetailsViewModel : MasterPageViewModel
    {
        private readonly HttpClient httpClient;

        public OrderWithStatus OrderWithStatus { get; set; }

        [FromRoute("id")]
        public int Id { get; set; }


        public OrderDetailsViewModel(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async override Task PreRender()
        {
            OrderWithStatus = await httpClient.GetJsonAsync<OrderWithStatus>($"orders/{Id}");

            await base.PreRender();
        }

    }
}

