using InventoryHub.Core.Domain.Entities;

namespace InventoryHub.Core.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(string id);
        Task<Product?> GetByIdAsync(string id);
        Task<List<Product>> GetAllAsync();
    }
}
