namespace StockFlow_Warehouse.Model;

using Microsoft.EntityFrameworkCore;

// TODO: Use compiled model so we can enable AOT and Trimming on publish
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public void SeedData()
    {
        if (Products.Any() || Categories.Any() || Warehouses.Any())
        {
            return;
        }

        var categories = new List<Category>
        {
            new Category { Name = "Foodstuffs" }
        };

        var products = new List<Product>
        {
            new Product { Name = "Baked Beans", Categories = categories },
            new Product { Name = "Choccy Cola", Categories = categories }
        };

        var warehouses = new List<Warehouse>
        {
            new Warehouse { Name = "Warehouse A", Address = "Kannikegade 18.1, DK-8200 Aarhus C" },
            new Warehouse { Name = "Warehouse B", Address = "Silkeborgvej 1, DK-8600 Silkeborg" }
        };

        Categories.AddRange(categories);
        Products.AddRange(products);
        Warehouses.AddRange(warehouses);

        var testList = products
            .Select(product => new ProductAmount { Product = product, Amount = 1 })
            .ToList();

        var testTransaction = new Transaction
        {
            Type = TransactionType.Move,
            From = warehouses[0],
            To = warehouses[1],
            Products = [new ProductAmount { Product = products[0], Amount = 1 }]
        };
        testTransaction.Products.AddRange(testList);
        Transactions.Add(testTransaction);

        SaveChanges();
    }
}