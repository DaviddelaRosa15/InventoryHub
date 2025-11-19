using InventoryHub.Core.Application.Interfaces.Repositories;
using InventoryHub.Core.Domain.Entities;
using InventoryHub.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace InventoryHub.Infrastructure.Persistence.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IDbContextFactory<ApplicationContext> _dbContext;
        public InventoryRepository(IDbContextFactory<ApplicationContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Inventory?> GetByProductIdAsync(string productId)
        {
            using var dbContext = _dbContext.CreateDbContext();

            return await dbContext.Inventories
                .FromSqlRaw("SELECT * FROM fn_inventory_get_by_productid({0})", productId)
                .FirstOrDefaultAsync();
        }
    }
}
