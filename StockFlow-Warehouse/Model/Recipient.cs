namespace StockFlow_Warehouse.Model;

public class Recipient
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; } = null!;
    public required string Address { get; set; } = null!;
}

public class Warehouse : Recipient
{
    public List<ProductAmount> Products { get; set; } = [];
}

public class Supplier : Recipient
{
}

public class Customer : Recipient
{
}