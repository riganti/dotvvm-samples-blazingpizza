using System;
using BlazingPizza.App;
using BlazingPizza.App.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDotVVM<DotvvmStartup>();
builder.Services.AddHttpClient("Api", client => 
{
    client.BaseAddress = builder.Configuration.GetValue<Uri>("Api:Url");
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<OrderStateService>();

var app = builder.Build();

// Build request pipeline
app.UseDotVVM<DotvvmStartup>(builder.Environment.ContentRootPath);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(builder.Environment.WebRootPath)
});

app.Run();