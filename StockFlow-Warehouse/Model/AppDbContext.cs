namespace StockFlow_Warehouse.Model;

using Microsoft.EntityFrameworkCore;

// TODO: Use compiled model so we can enable AOT and Trimming on publish
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Recipient> Recipients { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<TransactionLine> TransactionLines { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.From)
            .WithMany()
            .HasForeignKey(t => t.FromId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.To)
            .WithMany()
            .HasForeignKey(t => t.ToId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasMany(t => t.LineItems)
            .WithOne(li => li.Transaction)
            .HasForeignKey(li => li.TransactionId);

        modelBuilder.Entity<TransactionLine>()
            .HasOne(t => t.Product)
            .WithMany()
            .HasForeignKey(t => t.ProductId);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.Categories)
            .WithMany(c => c.Products);

        modelBuilder.Entity<Recipient>()
            .HasMany(w => w.Inventory)
            .WithOne(w => w.Warehouse)
            .HasForeignKey(i => i.WarehouseId);

        modelBuilder.Entity<InventoryItem>()
            .HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId);
    }

    public void SeedData()
    {
        if (Products.Any() || Categories.Any() || Recipients.Any() || Transactions.Any())
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

        var warehouses = new List<Recipient>
        {
            new()
            {
                Name = "Warehouse A",
                Address = "Kannikegade 18.1, DK-8200 Aarhus C", 
                Type = RecipientType.Warehouse
            },
            new()
            {
                Name = "Warehouse B", 
                Address = "Silkeborgvej 1, DK-8600 Silkeborg", 
                Type = RecipientType.Warehouse
            }
        };

        warehouses.ForEach(warehouse =>
            warehouse.Inventory = products
                .Select(product => new InventoryItem(warehouse, product, 100)).ToList());

        Categories.AddRange(categories);
        Products.AddRange(products);
        Recipients.AddRange(warehouses);

        var testTransaction = new Transaction
        {
            Type = TransactionType.Move,
            From = warehouses[0],
            To = warehouses[1]
        };

        testTransaction.LineItems =
        [
            new TransactionLine(products[0], testTransaction, 1),
            new TransactionLine(products[1], testTransaction, 1)
        ];

        Transactions.Add(testTransaction);

        SaveChanges();
    }
}