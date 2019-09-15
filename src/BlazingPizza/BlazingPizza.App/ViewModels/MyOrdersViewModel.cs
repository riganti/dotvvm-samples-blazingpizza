using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;

namespace BlazingPizza.App.ViewModels
{
    public class MyOrdersViewModel : MasterPageViewModel
    {
        private readonly HttpClient httpClient;


        public List<OrderWithStatus> Orders { get; set; }


        public MyOrdersViewModel(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async override Task PreRender()
        {
            Orders = await httpClient.GetJsonAsync<List<OrderWithStatus>>("orders");

            await base.PreRender();
        }

    }
}

