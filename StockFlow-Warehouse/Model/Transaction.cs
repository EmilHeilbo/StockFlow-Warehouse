namespace StockFlow_Warehouse.Model;

public enum TransactionType
{
    SALE,
    PURCHASE,
    RETURN,
    MOVE
}

public record struct Transaction(TransactionType Type, Recipient From, Recipient To, Dictionary<Product, int>? Products = null)
{
    public Guid Id { get; init; }
        = Guid.NewGuid();
    public TransactionType Type { get; set; }
        = Type;
    public Recipient From { get; set; }
        = From;
    public Recipient To { get; set; }
        = To;
    public Dictionary<Product, int>Products { get; set; }
        = Products ?? [];
};

