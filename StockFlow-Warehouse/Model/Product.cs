namespace StockFlow_Warehouse.Model;

public record struct Category(string Name)
{
    public Guid Id { get; }
        = Guid.NewGuid();
    public required string Name { get; set; }
        = Name;
};

public record struct Product(string Name, List<Category>? Categories = null)
{
    public Guid Id { get; }
        = Guid.NewGuid();
    public required string Name { get; set; }
        = Name;
    public List<Category> Categories { get; set; }
        = Categories ?? [];
};
