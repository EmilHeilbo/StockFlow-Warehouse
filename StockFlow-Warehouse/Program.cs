using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StockFlow_Warehouse.Model;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("StockFlow"));
}
else
{
    // TODO: fetch username/password or token from environment variables instead of storing them in plaintext
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    context.SeedData();
}

// TODO: This doesn't fetch objects recursively; I'll leave this for others to figure out ^u^

var productApi = app.MapGroup("/api/products");
productApi.MapGet("/", async (AppDbContext db) =>
        await db.Products.ToListAsync())
    .WithName("GetProducts");

productApi.MapGet("/{id}", async Task<Results<Ok<Product>, NotFound>> (string id, AppDbContext db) =>
        await db.Products.FirstOrDefaultAsync(a
            => a.Id.ToString() == id) is { } product
            ? TypedResults.Ok(product)
            : TypedResults.NotFound())
    .WithName("GetProductById");

var warehousesApi = app.MapGroup("/api/warehouses");
warehousesApi.MapGet("/", async (AppDbContext db) =>
        await db.Warehouses.ToListAsync())
    .WithName("GetWarehouses");

warehousesApi.MapGet("/{id}", async Task<Results<Ok<Warehouse>, NotFound>> (string id, AppDbContext db) =>
        await db.Warehouses.FirstOrDefaultAsync(a
            => a.Id.ToString() == id) is { } warehouse
            ? TypedResults.Ok(warehouse)
            : TypedResults.NotFound())
    .WithName("GetWarehouseById");

app.Run();

[JsonSerializable(typeof(Product[]))]
[JsonSerializable(typeof(List<Product>))]
[JsonSerializable(typeof(Category))]
[JsonSerializable(typeof(Warehouse))]
[JsonSerializable(typeof(Transaction))]
[JsonSerializable(typeof(ProductAmount))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}