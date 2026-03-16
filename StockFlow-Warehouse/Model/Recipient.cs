using System.ComponentModel.DataAnnotations;

namespace StockFlow_Warehouse.Model;

public class Recipient
{
    public Guid Id { get; init; } = Guid.NewGuid();
    [MaxLength(100)]
    public required string Name { get; set; } = null!;
    [MaxLength(100)]
    public required string Address { get; set; } = null!;
    [MaxLength(20), Phone]
    public string? PhoneNumber { get; set; }
    [MaxLength(50), EmailAddress]
    public string? Email { get; set; }
}

public class Warehouse : Recipient
{
    public List<InventoryItem> Inventory { get; set; } = [];
}

public class Supplier : Recipient
{
}

public class Customer : Recipient
{
}