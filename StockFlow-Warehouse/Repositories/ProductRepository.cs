namespace StockFlow_Warehouse.Repositories;

using Microsoft.EntityFrameworkCore;
using StockFlow_Warehouse.Repositories;
using StockFlow_Warehouse.Model;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db)
    {
        _db = db;
    }

    private IQueryable<Product> WithIncludes() =>
        _db.Products
            .Include(p => p.Categories);

    public Task<List<Product>> GetAll() => 
        WithIncludes().ToListAsync();

    public Task<Product?> GetById(Guid id) =>
        WithIncludes().FirstOrDefaultAsync(a
            => a.Id == id);

    public Task<List<Product>> GetAllInCategory(Category category) =>
        WithIncludes().Where(p => p.Categories.Contains(category)).ToListAsync();

    // TODO: Get inclusive/exclusive multiple category lists.

    public async Task Create(Product product)
    {
        await _db.AddAsync(product).AsTask();
        await _db.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var product = await GetById(id);
        if(product != null)
        {
            _db.Remove(product);
            await _db.SaveChangesAsync();
        }
    }

    public async Task Update(Product product)
    {
        Guid id = product.Id;
        var dbProduct = await GetById(id);
        if (dbProduct != null)
        {
            _db.Entry(dbProduct).CurrentValues.SetValues(product);
            await _db.SaveChangesAsync();
        }
    }
}