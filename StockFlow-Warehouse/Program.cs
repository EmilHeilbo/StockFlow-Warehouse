using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using StockFlow_Warehouse.Model;
using StockFlow_Warehouse.Repositories;

var builder = WebApplication.CreateSlimBuilder(args);

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

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

app.MapIdentityApi<IdentityUser>();

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
        await context.Database.EnsureDeletedAsync();
    await context.Database.MigrateAsync();
    await context.SeedDataAsync();
    await context.SeedRolesAsync(scope.ServiceProvider);
}

var productApi = app.MapGroup("/api/products");
productApi.MapGet("/", async (IProductRepository repo) =>
        await repo.GetAll())
    .WithName("GetProducts");


productApi.MapGet("/{id}", async Task<Results<Ok<Product>, NotFound>> (string id, IProductRepository repo) =>
        {
            Guid guid = Guid.Parse(id);
            return await repo.GetById(guid) is { } product
            ? TypedResults.Ok(product)
            : TypedResults.NotFound();
        })
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
    .WithName("GetTransactions")
    .RequireAuthorization();

transactionsApi.MapGet("/orders", async (AppDbContext db) =>
        await db.Transactions
            .Where(t => t.Type == TransactionType.Sale || t.Type == TransactionType.Return)
            .Include(t => t.LineItems)
            .ThenInclude(l => l.Product)
            .ToListAsync())
    .WithName("GetOrders");

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