using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockFlow_Warehouse.Model;
using StockFlow_Warehouse.Repositories;

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

builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

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

var api = app.MapGroup("/api");


api.MapGet("/warehouses", async Task<Results<Ok<List<Warehouse>>, NotFound>> ([FromServices] IWarehouseRepository repo) =>
        await repo.GetAll() is { } warehouseList
        ? TypedResults.Ok(warehouseList)
        : TypedResults.NotFound())
    .WithName("GetWarehouses");



   api.MapGet("/customers", async Task<Results<Ok<List<Customer>>, NotFound>> ([FromServices] ICustomerRepository repo) =>
        await repo.GetAll() is { } customerList
        ? TypedResults.Ok(customerList)
        : TypedResults.NotFound())
    .WithName("GetCustomers");


api.MapGet("/suppliers", async Task<Results<Ok<List<Supplier>>, NotFound>> ([FromServices]  ISupplierRepository repo) =>
        await repo.GetAll() is { } supplierList
        ? TypedResults.Ok(supplierList)
        : TypedResults.NotFound())
    .WithName("GetSuppliers");



api.MapGet("/products", async Task<Results<Ok<List<Product>>, NotFound>> ([FromServices] IProductRepository repo, AppDbContext db, [FromQuery(Name = "categoryId")] Guid? categoryId) =>
        {
            if(categoryId == null)
            {
                return await repo.GetAll() is { } productList
                ? TypedResults.Ok(productList)
                : TypedResults.NotFound();
            } else
            {
                //Implement
                List<Category> categories = await db.Categories.ToListAsync();
                var matchingCategory = categories.Find(c => c.Id == categoryId);
                var matchId = matchingCategory != null ? matchingCategory.Id : Guid.Empty;
                return await repo.GetById(matchId) is { } product
                ? TypedResults.Ok(new List<Product>{product})
                : TypedResults.NotFound();
            }
            
        })
    .WithName("GetProducts");

//Not ideal, but we don't have repository for category
api.MapGet("/categories", async Task<Results<Ok<List<Category>>, NotFound>> (AppDbContext db) =>
        await db.Categories.ToListAsync() is { } warehouseList
        ? TypedResults.Ok(warehouseList)
        : TypedResults.NotFound())
    .WithName("GetCategories");

api.MapGet("/transactions", async Task<Results<Ok<List<Transaction>>, NotFound>> ([FromServices] ITransactionRepository repo) =>
        await repo.GetAll() is { } warehouseList
        ? TypedResults.Ok(warehouseList)
        : TypedResults.NotFound())
    .WithName("GetTransactions");
/*


api.MapGet("/", async (AppDbContext db) =>
        await db.Products.ToListAsync());
api.MapGet("/", async (IProductRepository repo) =>
        await repo.GetAll())
    .WithName("GetProducts");


api.MapGet("/{id}", async Task<Results<Ok<Product>, NotFound>> (string id, IProductRepository repo) =>
        {
            Guid guid = Guid.Parse(id);
            return await repo.GetById(guid) is { } product
            ? TypedResults.Ok(product)
            : TypedResults.NotFound();
        })
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
*/
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