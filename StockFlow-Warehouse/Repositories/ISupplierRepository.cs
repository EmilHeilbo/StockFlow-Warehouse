namespace StockFlow_Warehouse.Repositories;

using StockFlow_Warehouse.Model;

public interface ISupplierRepository
{
    Task<List<Supplier>> GetAll();
    Task<Supplier> GetById(Guid id);
    Task Create(Supplier supplier);
    Task Delete(Guid id);
    Task Update(Supplier supplier);

}