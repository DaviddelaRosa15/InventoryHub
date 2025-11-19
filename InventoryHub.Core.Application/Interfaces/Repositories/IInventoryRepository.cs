using InventoryHub.Core.Domain.Entities;

namespace InventoryHub.Core.Application.Interfaces.Repositories
{
    public interface IInventoryRepository
    {
        Task<Inventory?> GetByProductIdAsync(string id);
    }
}
