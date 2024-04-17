using System.Net.Http;
using System.Threading.Tasks;
using BlazingPizza.App.Services;

namespace BlazingPizza.App.ViewModels;

public class CheckoutViewModel(IHttpClientFactory httpClientFactory, OrderStateService orderStateService)
    : MasterPageViewModel
{
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
        using var httpClient = httpClientFactory.CreateClient("Api");
        var newOrderId = await httpClient.PostJsonAsync<int>("orders", Order);
        orderStateService.SaveCurrentOrderState(new Order());
        Context.RedirectToRoute("OrderDetails", new { Id = newOrderId });
    }
}