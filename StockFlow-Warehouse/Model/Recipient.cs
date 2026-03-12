namespace StockFlow_Warehouse.Model;

public record Recipient(string Name, string Address)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; } = Name;
    public string Address { get; set; } = Address;
};


public record Warehouse(string Name, string Address, List<ProductAmount>? Products = null)
    : Recipient(Name: Name, Address: Address)
{
    List<ProductAmount> Products { get; set; }
        = Products ?? [];
};
public record Supplier(string Name, string Address)
    : Recipient(Name: Name, Address: Address);
public record Customer(string Name, string Address)
    : Recipient(Name: Name, Address: Address);


