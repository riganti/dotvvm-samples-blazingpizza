using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazingPizza.App.ViewModels;

public class MyOrdersViewModel(IHttpClientFactory httpClientFactory) : MasterPageViewModel
{
    public List<OrderWithStatus> Orders { get; set; }

    public override async Task PreRender()
    { 
        using var httpClient = httpClientFactory.CreateClient("Api");
        Orders = await httpClient.GetJsonAsync<List<OrderWithStatus>>("orders");

        await base.PreRender();
    }
}