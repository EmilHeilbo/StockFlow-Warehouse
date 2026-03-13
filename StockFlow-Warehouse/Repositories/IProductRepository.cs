namespace StockFlow_Warehouse.Repositories;

using StockFlow_Warehouse.Model;

public interface IProductRepository
{
    Task<List<Product>> getAll();
}