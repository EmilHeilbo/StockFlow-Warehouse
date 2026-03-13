namespace StockFlow_Warehouse.Repositories;

using StockFlow_Warehouse.Model;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetAll();
    Task<Transaction> GetById(Guid id);
    Task<List<Transaction>> GetAllOfType(TransactionType type);
    Task Create(Transaction transaction);
    Task Delete(Guid id);
    Task Update(Transaction transaction);

}