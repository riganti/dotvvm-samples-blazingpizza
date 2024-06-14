using System.Linq;
using System.Net.Mime;
using BlazingPizza.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddDbContext<PizzaStoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB"), options =>
    {
        options.EnableRetryOnFailure();
    });
});
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes
        .Concat(new[] { MediaTypeNames.Application.Octet });
});

// Build request pipeline
var app = builder.Build();
app.UseResponseCompression();
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Run database migrations
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<PizzaStoreContext>();
if (db.Database.EnsureCreated())
{
    SeedData.Initialize(db);
}

app.Run();