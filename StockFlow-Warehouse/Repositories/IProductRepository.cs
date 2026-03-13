namespace StockFlow_Warehouse.Repositories;

using StockFlow_Warehouse.Model;

public interface IProductRepository
{
    Task<List<Product>> GetAll();
    Task<Product?> GetById(Guid id);
    Task<List<Product>> GetAllInCategory(Category category);
    Task Create(Product product);
    Task Delete(Guid id);
    Task Update(Product product);

}