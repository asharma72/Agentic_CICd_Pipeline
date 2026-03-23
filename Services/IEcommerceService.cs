namespace Ecommerce.API.Services
{
    public interface IEcommerceService<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }

    public interface IProductService : IEcommerceService<Product>
    {
    }

    public interface IOrderService : IEcommerceService<Order>
    {
    }

    public interface ICustomerService : IEcommerceService<Customer>
    {
    }
}