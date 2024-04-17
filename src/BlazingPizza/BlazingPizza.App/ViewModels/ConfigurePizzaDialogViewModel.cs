using DotVVM.Framework.ViewModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazingPizza.App.ViewModels;

public class ConfigurePizzaDialogViewModel(IHttpClientFactory httpClientFactory) : DotvvmViewModelBase
{
    public List<Topping> Toppings { get; set; } = new();

    public Pizza ConfiguringPizza { get; set; }

    public Topping SelectedTopping { get; set; }

    public event Action<Pizza> OnConfigured;

    public override async Task PreRender()
    {
        if (!Context.IsPostBack)
        {
            using var httpClient = httpClientFactory.CreateClient("Api");
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