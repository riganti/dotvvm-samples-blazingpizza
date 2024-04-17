using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BlazingPizza.App.Services;

namespace BlazingPizza.App.ViewModels;

public class GetPizzaViewModel(IHttpClientFactory httpClientFactory, OrderStateService orderStateService)
    : MasterPageViewModel
{
    public List<PizzaSpecial> Specials { get; set; } = new();

    public Order Order { get; set; }

    public ConfigurePizzaDialogViewModel ConfiguringPizzaDialog { get; set; } = new(httpClientFactory);

    public override Task Init()
    {
        ConfiguringPizzaDialog.OnConfigured += AddPizza;

        return base.Init();
    }

    public override async Task PreRender()
    {
        if (!Context.IsPostBack)
        {
            using var httpClient = httpClientFactory.CreateClient("Api");
            Specials = await httpClient.GetJsonAsync<List<PizzaSpecial>>("specials");

            Order = orderStateService.LoadCurrentOrderState() ?? new Order();
        }
        await base.PreRender();
    }

    public void ShowConfigurePizzaDialog(PizzaSpecial special)
    {
        ConfiguringPizzaDialog.Show(special);
    }

    private void AddPizza(Pizza pizza)
    {
        Order.Pizzas.Add(pizza);
        orderStateService.SaveCurrentOrderState(Order);
    }

    public void RemovePizza(Pizza pizza)
    {
        Order.Pizzas.Remove(pizza);
        orderStateService.SaveCurrentOrderState(Order);
    }

    public void Checkout()
    {
        orderStateService.SaveCurrentOrderState(Order);
        Context.RedirectToRoute("checkout");
    }
}