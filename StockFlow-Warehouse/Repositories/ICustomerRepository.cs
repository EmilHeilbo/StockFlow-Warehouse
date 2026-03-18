namespace StockFlow_Warehouse.Repositories;

using StockFlow_Warehouse.Model;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAll();
    Task<Customer> GetById(Guid id);
    Task Create(Customer customer);
    Task Delete(Guid id);
    Task Update(Customer customer);

}