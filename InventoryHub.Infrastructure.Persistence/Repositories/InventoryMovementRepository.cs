using InventoryHub.Core.Application.Dtos.InventoryMovement;
using InventoryHub.Core.Application.Interfaces.Repositories;
using InventoryHub.Core.Domain.Entities;
using InventoryHub.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace InventoryHub.Infrastructure.Persistence.Repositories
{
    public class InventoryMovementRepository : IInventoryMovementRepository
    {
        private readonly IDbContextFactory<ApplicationContext> _dbContext;
        public InventoryMovementRepository(IDbContextFactory<ApplicationContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task RegisterEntryAsync(InventoryMovement movement)
        {
            using var dbContext = _dbContext.CreateDbContext();

            await dbContext.Database.ExecuteSqlRawAsync(
                "CALL sp_register_entry({0}, {1}, {2}, {3})",
                movement.Id, movement.ProductId, movement.Quantity, movement.CreatedBy);
        }

        public async Task<InventoryInfoDTO> GetInventoryInfoAsync(string productId)
        {
            using var dbContext = _dbContext.CreateDbContext();
            var result = await dbContext.Database
                .SqlQueryRaw<InventoryInfoDTO>(
                    "SELECT * FROM fn_get_inventory_info({0})", productId)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task RegisterExitAsync(InventoryMovement movement)
        {
            using var dbContext = _dbContext.CreateDbContext();
            await dbContext.Database
                .ExecuteSqlRawAsync(
                    "CALL sp_register_exit({0}, {1}, {2}, {3})",
                    movement.Id, movement.ProductId, movement.Quantity, movement.CreatedBy);
        }
    }
}
