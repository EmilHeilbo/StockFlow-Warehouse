using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using StockFlow_Warehouse.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// TODO: fetch username/password or token from environment variables instead of storing them in plaintext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLite")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Running in development mode");
    app.MapOpenApi();
    app.MapScalarApiReference();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (app.Environment.IsDevelopment())
        context.Database.EnsureDeleted();
    context.Database.Migrate();
    context.SeedData();
}

var productApi = app.MapGroup("/api/products");
productApi.MapGet("/", async (AppDbContext db) =>
        await db.Products
            .Include(p => p.Categories)
            .ToListAsync())
    .WithName("GetProducts");

productApi.MapGet("/{id}", async Task<Results<Ok<Product>, NotFound>> (string id, AppDbContext db) =>
        await db.Products.FirstOrDefaultAsync(a
            => a.Id.ToString() == id) is { } product
            ? TypedResults.Ok(product)
            : TypedResults.NotFound())
    .WithName("GetProductById");

var warehousesApi = app.MapGroup("/api/warehouses");
warehousesApi.MapGet("/", async (AppDbContext db) =>
        await db.Recipients
            .Where(r => r.Type == RecipientType.Warehouse)
            .Include(w => w.Inventory)
            .ThenInclude(i => i.Product)
            .ThenInclude(p => p.Categories)
            .ToListAsync())
    .WithName("GetWarehouses");

warehousesApi.MapGet("/{id}", async Task<Results<Ok<Recipient>, NotFound>> (string id, AppDbContext db) =>
        await db.Recipients
            .Where(r => r.Type == RecipientType.Warehouse)
            .FirstOrDefaultAsync(a
            => a.Id.ToString() == id) is { } warehouse
            ? TypedResults.Ok(warehouse)
            : TypedResults.NotFound())
    .WithName("GetWarehouseById");

var transactionsApi = app.MapGroup("/api/transactions");
transactionsApi.MapGet("/", async (AppDbContext db) =>
        await db.Transactions
            .Include(t => t.LineItems)
            .ThenInclude(l => l.Product)
            .ToListAsync())
    .WithName("GetTransactions");

app.Run();

[JsonSerializable(typeof(Product))]
[JsonSerializable(typeof(Category))]
[JsonSerializable(typeof(Recipient))]
[JsonSerializable(typeof(Transaction))]
[JsonSerializable(typeof(InventoryItem))]
[JsonSerializable(typeof(TransactionLine))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}