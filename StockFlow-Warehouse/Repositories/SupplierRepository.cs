using Microsoft.EntityFrameworkCore;
using StockFlow_Warehouse.Repositories;
using StockFlow_Warehouse.Model;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _db;

    public SupplierRepository(AppDbContext db)
    {
        _db = db;
    }

    private IQueryable<Supplier> WithIncludes() =>
        _db.Suppliers;

    public Task<List<Supplier>> GetAll() =>
        WithIncludes().ToListAsync();

    public Task<Supplier?> GetById(Guid id) =>
        WithIncludes().FirstOrDefaultAsync(a
            => a.Id == id);

    public async Task Create(Supplier supplier)
    {
        await _db.AddAsync(supplier).AsTask();
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

    public async Task Update(Supplier supplier)
    {
        Guid id = supplier.Id;
        var dbProduct = await GetById(id);
        if (dbProduct != null)
        {
            _db.Entry(dbProduct).CurrentValues.SetValues(supplier);
            await _db.SaveChangesAsync();
        }
    }
}