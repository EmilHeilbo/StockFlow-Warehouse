using System.ComponentModel.DataAnnotations;

namespace StockFlow_Warehouse.Model;

public class Recipient
{
    public Guid Id { get; init; } = Guid.NewGuid();
    [MaxLength(100)]
    public required string Name { get; set; } = null!;
    [MaxLength(100)]
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