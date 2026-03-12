namespace StockFlow_Warehouse.Model;

public class Category
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
}

public class Product
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public List<Category> Categories { get; set; } = [];
}

public class ProductAmount
{
    // Lazy workaround to EF Core requiring all model types to have a primary key by default
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Product Product { get; set; }
    public int Amount { get; set; }
}