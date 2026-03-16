using System.ComponentModel.DataAnnotations;

namespace StockFlow_Warehouse.Model;

public class Category
{
    public Guid Id { get; init; } = Guid.NewGuid();
    [MaxLength(100)]
    public required string Name { get; set; }
}

public class Product
{
    public Guid Id { get; init; } = Guid.NewGuid();
    [MaxLength(100)]
    public required string Name { get; set; }

    public decimal Price { get; set; } = 0;
    [MaxLength(14)]
    [RegularExpression("^[0-9]{0,14}$")]
    public string Barcode { get; set; } = "";
    [MaxLength(1200)]
    public string Description { get; set; } = "";
    public List<Category> Categories { get; set; } = [];
}

public class ProductAmount
{
    // Lazy workaround to EF Core requiring all model types to have a primary key by default
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Product Product { get; set; }
    public required int Amount { get; set; }
    public decimal TotalPrice => Product.Price * Amount;
}