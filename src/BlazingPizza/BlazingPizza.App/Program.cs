using System;
using BlazingPizza.App;
using BlazingPizza.App.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container
builder.Services.AddDotVVM<DotvvmStartup>();
builder.Services.AddHttpClient("Api", client => 
{
    client.BaseAddress = new Uri("http://blazingpizza-server");
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<OrderStateService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Build request pipeline
app.UseDotVVM<DotvvmStartup>(builder.Environment.ContentRootPath);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(builder.Environment.WebRootPath)
});

app.Run();