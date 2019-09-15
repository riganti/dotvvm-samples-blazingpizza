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
    public class GetPizzaViewModel : MasterPageViewModel
    {
        private readonly HttpClient httpClient;
        private readonly OrderStateService orderStateService;


        public List<PizzaSpecial> Specials { get; set; } = new List<PizzaSpecial>();

        public Order Order { get; set; }

        public ConfigurePizzaDialogViewModel ConfiguringPizzaDialog { get; set; }


        public GetPizzaViewModel(HttpClient httpClient, OrderStateService orderStateService)
        {
            this.httpClient = httpClient;
            this.orderStateService = orderStateService;

            ConfiguringPizzaDialog = new ConfigurePizzaDialogViewModel(httpClient);
            ConfiguringPizzaDialog.OnConfigured += AddPizza;
        }

        public async override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
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
            Context.RedirectToRoute("Checkout");
        }
    }
}

