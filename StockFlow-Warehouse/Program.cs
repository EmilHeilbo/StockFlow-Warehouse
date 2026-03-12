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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("SQLite")));
}
else
{
    // TODO: Either fetch username/password from env variables or store them in `appsettings.json`
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

List<Category> categories =
[
    new Category { Name = "Foodstuffs" }
];

List<Product> sampleProducts =
[
    new Product { Name = "Baked Beans", Categories = categories },
    new Product { Name = "Choccy Cola", Categories = categories }
];

var testList = sampleProducts
    .Select(product => new ProductAmount { Product = product, Amount = 1 })
    .ToList();

List<Warehouse> warehouses =
[
    new Warehouse { Name = "Warehouse A", Address = "Kannikegade 18.1, DK-8200 Aarhus C" },
    new Warehouse { Name = "Warehouse B", Address = "Silkeborgvej 1, DK-8600 Silkeborg" }
];

var testTransaction = new Transaction { Type = TransactionType.Move, From = warehouses[0], To = warehouses[1] };
testTransaction.Products.AddRange(testList);

var productsApi = app.MapGroup("/api/products");
productsApi.MapGet("/", () => sampleProducts)
    .WithName("GetProducts");

productsApi.MapGet("/{id}", Results<Ok<Product>, NotFound> (string id) =>
        sampleProducts.FirstOrDefault(a
            => a.Id.ToString() == id) is { } product
            ? TypedResults.Ok(product)
            : TypedResults.NotFound())
    .WithName("GetProductById");

app.Run();

[JsonSerializable(typeof(Product[]))]
partial class AppJsonSerializerContext : JsonSerializerContext
{
}