# Blazing Pizza in DotVVM

This sample is a rewrite of the popular [BlazingPizza sample](https://github.com/dotnet-presentations/blazor-workshop) into [DotVVM](https://github.com/riganti/dotvvm). 

It demonstrates:

* differences between the Razor/Blazor and DotVVM syntax.
* usage of DotVVM with ASP.NET Core 3.0.

## Running the sample app

1. Clone the repository using command git clone `https://github.com/riganti/dotvvm-samples-blazingpizza.git`

2. Open the solution (BlazingPizza.sln) in Visual Studio

3. Set BlazingPizza.Server and BlazingPizza.Server as your startup projects

![Multiple startups](/screenshots/multiple_startups.png?raw=true "Multiple startup project setup")

4. Run BlazingPizza.Server and BlazingPizza.Server projects

## Differences between Razor and DotVVM syntax

The syntax of DotVVM views is quite different from the Razor/Blazor approach because of the MVVM (Model-View-ViewModel) pattern.

DotVVM ships with many [built-in controls](https://dotvvm.com/docs/controls/builtin/Button/latest) (e.g. `<dot:Button ...>`) and uses [data-binding expressions](https://www.dotvvm.com/docs/tutorials/basics-value-binding/latest) (e.g. `Text="{value: FirstName}"` instead of `@` code blocks.

Here is the list of the most frequent differences:

### Outputting text in the page

Razor uses `@expression` to print values in the page.

In DotVVM, you can use `{{value: expression}}` to do the same thing. In contrast to Razor, you can use only simple expressions without method calls - DotVVM translates these expressions in JavaScript. See the [supported expressions](https://www.dotvvm.com/docs/tutorials/basics-value-binding/latest) for value bindings.


### `Visible` instead of `@if`

In DotVVM, you can use `Visible` property on any HTML element to hide it from the page. It is similar to `@if` expressions in Razor.

```
// Razor
@if (ordersWithStatus.Count == 0)
{
  <div class="order-list">
    <h2>No orders placed</h2>
  </div>
}

// DotVVM
<div class="order-list" Visible="{value: Orders.Count == 0}">
  <h2>No orders placed</h2>
</div>
```

There is also `IncludedInPage` property that can be used instead of `Visible`. `Visible` only hides the element by setting `display: none` while `IncludedInPage` removes the element from DOM completely.


### `<dot:Repeater>` instead of `@foreach`

Instead of using loops to iterate over a collection, DotVVM has the `<dot:Repeater>` control which can be data-bound to a collection in the viewmodel.

```
// Razor
<ul>
	@foreach (var topping in pizza.Toppings)
	{
		<li>+ @topping.Name</li>
	}
</ul>

// DotVVM
<dot:Repeater DataSource="{value: Toppings}"
              WrapperTagName="ul">
	<li>+ {{value: Name}}</li>
</ul>
```

The `WrapperTagName` property specifies the name of the HTML element that should wrap the individual items - `<ul>` in this case.

The `Repeater` also has the `EmptyDataTemplate` property that can be used to define content displayed when the collection is empty.


### Navigation between pages

We don't like to use plain hyperlinks to link to other DotVVM pages. 

Instead, we encourage you to use `<dot:RouteLink>` control - it can specify `RouteName` and route parameters. This approach is more resillient to route URL changes.

```
// Razor
<a href="myorders/@item.Order.OrderId" class="btn btn-success">
	Track &gt;
</a>

// DotVVM
<dot:RouteLink RouteName="OrderDetails" Param-id="{value: Order.OrderId}"
			   class="btn btn-success">
	Track &gt;
</dot:RouteLink>
```

The routes are registered in the `DotvvmStartup` file:

```
config.RouteTable.Add("OrderDetails", "myorders/{id:int}", "Views/OrderDetails.dothtml");
```

### Calling methods in the viewmodel

We use [command binding](https://www.dotvvm.com/docs/tutorials/basics-command-binding/latest) to call methods defined in the viewmodel. 

```
// Razor
<button type="button" class="delete-topping" 
        @onclick="@(() => RemoveTopping(topping))">x</button>
				
// DotVVM
<dot:Button class="delete-topping"
            Click="{command: _root.RemoveTopping(_this)}">x</dot:Button>
```

### Working with CSS classes

Instead of concatenating list of CSS classes on an element, you can use `class-something` binding to define conditions indicating whether the CSS class is applied or not.

```
// Razor
<div class="order-total @(Order.Pizzas.Count > 0 ? "" : "hidden")">
	Total: ...
</div>

// DotVVM
<div class="order-total" class-hidden="{value: Order.Pizzas.Count > 0}">
	Total: ...
</div>
```

### Validation

DotVVM uses the same DataAnnotation attributes like Blazor and other .NET frameworks. You can use `<dot:Validator>` or `<dot:ValidationSummary>` controls to display validation errors.

```
// Razor
<InputText @bind-Value="Address.PostalCode" />
<ValidationMessage For="@(() => Address.PostalCode)" />

// DotVVM
<dot:TextBox Text="{value: PostalCode}" />
<dot:Validator Value="{value: PostalCode}" ShowErrorMessageText="true" />
```

[DotVVM validation](https://www.dotvvm.com/docs/tutorials/basics-validation/latest) offers plenty of configuration options and flexibility to add CSS classes, display tooltips, or applying other behaviors for invalid viewmodel properties.

---

## Other resources

* [DotVVM.com](https://www.dotvvm.com)
* [DotVVM GitHub](https://github.com/riganti/dotvvm)
* [DotVVM Blog](https://www.dotvvm.com/blog)
* [Documentation](https://www.dotvvm.com/docs)
* [Twitter @dotvvm](https://twitter.com/dotvvm)
* [Gitter Chat](https://gitter.im/riganti/dotvvm)
* [Samples](https://github.com/search?q=topic%3Adotvvm-sample+org%3Ariganti&type=Repositories)
* [Roadmap](https://github.com/riganti/dotvvm/blob/master/roadmap.md)
