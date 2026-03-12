namespace StockFlow_Warehouse.Model;

public enum TransactionType
{
    Sale,
    Purchase,
    Return,
    Move
}

public class Transaction
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required TransactionType Type { get; init; }
    public required Recipient From { get; init; }
    public required Recipient To { get; init; }
    public List<ProductAmount> Products { get; init; } = [];
};