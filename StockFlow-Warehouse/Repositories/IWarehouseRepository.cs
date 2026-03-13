namespace StockFlow_Warehouse.Repositories;

using StockFlow_Warehouse.Model;

public interface IWarehouseRepository
{
    Task<List<Warehouse>> GetAll();
    Task<Warehouse> GetById(Guid id);
    Task Create(Warehouse warehouse);
    Task Delete(Guid id);
    Task Update(Warehouse warehouse);

}