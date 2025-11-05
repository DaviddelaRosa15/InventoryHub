using InventoryHub.Core.Domain.Entities;

namespace InventoryHub.Core.Application.Interfaces.Repositories
{
    public interface IInventoryMovementRepository
    {
        Task RegisterEntryAsync(InventoryMovement movement);
    }
}
