namespace StockFlow_Warehouse.Repositories;

using Microsoft.EntityFrameworkCore;
using StockFlow_Warehouse.Repositories;
using StockFlow_Warehouse.Model;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _db;

    public CustomerRepository(AppDbContext db)
    {
        _db = db;
    }

    private IQueryable<Customer> WithIncludes() =>
        _db.Customers;

    public Task<List<Customer>> GetAll() =>
        WithIncludes().ToListAsync();

    public Task<Customer?> GetById(Guid id) =>
        WithIncludes().FirstOrDefaultAsync(a
            => a.Id == id);

    public async Task Create(Customer customer)
    {
        await _db.AddAsync(customer).AsTask();
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

    public async Task Update(Customer customer)
    {
        Guid id = customer.Id;
        var dbProduct = await GetById(id);
        if (dbProduct != null)
        {
            _db.Entry(dbProduct).CurrentValues.SetValues(customer);
            await _db.SaveChangesAsync();
        }
    }
}