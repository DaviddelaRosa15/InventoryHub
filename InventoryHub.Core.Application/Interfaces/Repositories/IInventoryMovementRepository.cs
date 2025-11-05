using InventoryHub.Core.Application.Dtos.InventoryMovement;
using InventoryHub.Core.Domain.Entities;

namespace InventoryHub.Core.Application.Interfaces.Repositories
{
    public interface IInventoryMovementRepository
    {
        Task RegisterEntryAsync(InventoryMovement movement);
        Task<InventoryInfoDTO> GetInventoryInfoAsync(string productId);
        Task RegisterExitAsync(InventoryMovement movement);
    }
}
