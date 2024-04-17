using System.Net.Http;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;

namespace BlazingPizza.App.ViewModels;

public class OrderDetailsViewModel(IHttpClientFactory httpClientFactory) : MasterPageViewModel
{
    public OrderWithStatus OrderWithStatus { get; set; }

    [FromRoute("id")]
    public int Id { get; set; }

    public override async Task PreRender()
    {
        using var httpClient = httpClientFactory.CreateClient("Api");
        OrderWithStatus = await httpClient.GetJsonAsync<OrderWithStatus>($"orders/{Id}");

        await base.PreRender();
    }

}