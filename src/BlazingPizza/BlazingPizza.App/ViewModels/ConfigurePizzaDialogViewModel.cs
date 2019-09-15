using DotVVM.Framework.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazingPizza.App.ViewModels
{
    public class ConfigurePizzaDialogViewModel : DotvvmViewModelBase
    {
        private readonly HttpClient httpClient;

        public List<Topping> Toppings { get; set; } = new List<Topping>();

        public Pizza ConfiguringPizza { get; set; }

        public Topping SelectedTopping { get; set; }

        public event Action<Pizza> OnConfigured;

        public ConfigurePizzaDialogViewModel(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }


        public async override Task PreRender()
        {
            if (!Context.IsPostBack)
            {
                Toppings = await httpClient.GetJsonAsync<List<Topping>>("toppings");
            }

            await base.PreRender();
        }


        public void AddTopping()
        {
            ConfiguringPizza.Toppings.Add(new PizzaTopping() { Topping = SelectedTopping });
        }

        public void RemoveTopping(PizzaTopping topping)
        {
            ConfiguringPizza.Toppings.Remove(topping);
        }

        public void Cancel()
        {
            ConfiguringPizza = null;
        }

        public void Show(PizzaSpecial special)
        {
            ConfiguringPizza = new Pizza()
            {
                Special = special,
                SpecialId = special.Id,
                Size = Pizza.DefaultSize,
                Toppings = new List<PizzaTopping>()
            };
        }

        public void Confirm()
        {
            OnConfigured?.Invoke(ConfiguringPizza);
            ConfiguringPizza = null;
        }


    }
}
