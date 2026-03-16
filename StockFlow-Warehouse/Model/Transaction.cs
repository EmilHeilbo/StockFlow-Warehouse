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

public class TransactionLine
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Product Product { get; set; }
    public required int Amount { get; set; }
    public required Transaction Transaction { get; init; }
    // Snapshot price to avoid later price changes affecting existing transactions
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Product.Price * Amount;
}
public class Transaction
{
    public Guid Id { get; init; } = Guid.NewGuid();
    // Consider inferring Type from type of From & To fields
    public required TransactionType Type { get; init; }
    public TransactionState State { get; set; } = TransactionState.Reserved;
    public Recipient? From { get; init; }
    public Recipient? To { get; init; }
    public List<TransactionLine> LineItems { get; init; } = [];
    [MaxLength(50)]
    public string TrackingId { get; set; } = "";
    public decimal TotalPrice => LineItems.Sum(p => p.TotalPrice);
};