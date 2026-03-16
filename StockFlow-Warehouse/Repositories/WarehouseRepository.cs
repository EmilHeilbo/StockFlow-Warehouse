namespace StockFlow_Warehouse.Repositories;

using Microsoft.EntityFrameworkCore;
using StockFlow_Warehouse.Repositories;
using StockFlow_Warehouse.Model;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly AppDbContext _db;

    public WarehouseRepository(AppDbContext db)
    {
        _db = db;
    }

    private IQueryable<Warehouse> WithIncludes() =>
        _db.Warehouses
            .Include(w => w.Products)
                .ThenInclude(pa => pa.Product)
                    .ThenInclude(p => p.Categories);

    public Task<List<Warehouse>> GetAll() =>
        WithIncludes().ToListAsync();

    public Task<Warehouse?> GetById(Guid id) =>
        WithIncludes().FirstOrDefaultAsync(a
            => a.Id == id);

    public async Task Create(Warehouse warehouse)
    {
        await _db.AddAsync(warehouse).AsTask();
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

    public async Task Update(Warehouse warehouse)
    {
        Guid id = warehouse.Id;
        var dbProduct = await GetById(id);
        if (dbProduct != null)
        {
            _db.Entry(dbProduct).CurrentValues.SetValues(warehouse);
            await _db.SaveChangesAsync();
        }
    }
}