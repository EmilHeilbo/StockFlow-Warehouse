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
    public DbSet<InventoryItem> LineItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Categories)
            .WithMany(c => c.Products);
        
        modelBuilder.Entity<Warehouse>()
            .HasMany(w => w.Inventory)
            .WithOne(p => p.Warehouse);
        
        modelBuilder.Entity<Transaction>()
            .HasMany(t => t.LineItems)
            .WithOne(li => li.Transaction);
    }

    public void SeedData()
    {
        if (Products.Any() || Categories.Any() || Warehouses.Any() || Transactions.Any())
        {
            return;
        }
        
        var categories = new List<Category>
        {
            new() { Name = "Foodstuffs" }
        };

        var products = new List<Product>
        {
            new() { Name = "Baked Beans", Categories = categories },
            new() { Name = "Choccy Cola", Categories = categories }
        };

        var warehouses = new List<Warehouse>
        {
            new() { Name = "Warehouse A", Address = "Kannikegade 18.1, DK-8200 Aarhus C" },
            new() { Name = "Warehouse B", Address = "Silkeborgvej 1, DK-8600 Silkeborg" }
        };

        Categories.AddRange(categories);
        Products.AddRange(products);
        Warehouses.AddRange(warehouses);

        var testList = products
            .Select(product => new TransactionLine { Product = product, Amount = 1, Transaction = null })
            .ToList();

        var testTransaction = new Transaction
        {
            Type = TransactionType.Move,
            From = warehouses[0],
            To = warehouses[1],
            LineItems = [new TransactionLine { Product = products[0], Amount = 1, Transaction = null }]
        };
        testTransaction.LineItems.AddRange(testList);
        Transactions.Add(testTransaction);

        SaveChanges();
    }
}