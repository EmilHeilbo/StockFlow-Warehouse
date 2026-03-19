using Microsoft.EntityFrameworkCore;
using StockFlow_Warehouse.Repositories;
using StockFlow_Warehouse.Model;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _db;

    public TransactionRepository(AppDbContext db)
    {
        _db = db;
    }

    private IQueryable<Transaction> WithIncludes() =>
        _db.Transactions;

    public Task<List<Transaction>> GetAll() =>
        WithIncludes().ToListAsync();

    public Task<Transaction?> GetById(Guid id) =>
        WithIncludes().FirstOrDefaultAsync(a
            => a.Id == id);
    public Task<List<Transaction>> GetAllOfType(TransactionType type) =>
        WithIncludes().ToListAsync(); //Incorrect, TODO implement correctly

    public async Task Create(Transaction transaction)
    {
        await _db.AddAsync(transaction).AsTask();
        await _db.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var product = await GetById(id);
        if (product != null)
        {
            _db.Remove(product);
            await _db.SaveChangesAsync();
        }
    }

    public async Task Update(Transaction transaction)
    {
        Guid id = transaction.Id;
        var dbProduct = await GetById(id);
        if (dbProduct != null)
        {
            _db.Entry(dbProduct).CurrentValues.SetValues(transaction);
            await _db.SaveChangesAsync();
        }
    }
}