using System.ComponentModel.DataAnnotations;

namespace StockFlow_Warehouse.Model;

public enum TransactionType
{
    Sale,
    Purchase,
    Return,
    Move
}

public enum TransactionState
{
    Reserved,
    InTransit,
    Delivered,
    Cancelled,
    Returned
}

public class Transaction
{
    public Guid Id { get; init; } = Guid.NewGuid();
    // Consider inferring Type from type of From & To fields
    public required TransactionType Type { get; init; }
    public TransactionState State { get; set; } = TransactionState.Reserved;
    public required Recipient From { get; init; }
    public required Recipient To { get; init; }
    public List<ProductAmount> Products { get; init; } = [];
    [MaxLength(50)]
    public string TrackingId { get; set; } = "";
    public decimal TotalPrice => Products.Sum(p => p.TotalPrice);
};